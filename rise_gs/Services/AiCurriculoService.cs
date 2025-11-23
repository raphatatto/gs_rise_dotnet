using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using rise_gs.DTOs;

namespace rise_gs.Services
{
    public class AiCurriculoService : IAiCurriculoService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly RiseContext _db;
        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public AiCurriculoService(HttpClient http, IConfiguration config, RiseContext db)
        {
            _http = http;
            _db = db;
            _apiKey = config["Gemini:ApiKey"] ?? "";
            _model = config["Gemini:Model"] ?? "gemini-2.5-flash";

            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Gemini ApiKey não configurada.");
        }

        private async Task<string> CallGeminiAsync(string prompt)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent";

            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Add("x-goog-api-key", _apiKey);

            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            req.Content = JsonContent.Create(body);

            var res = await _http.SendAsync(req);
            var json = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                throw new InvalidOperationException($"Erro Gemini: {(int)res.StatusCode} - {json}");

            using var doc = JsonDocument.Parse(json);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "";
        }

        private static string ToJsonSafe(object? o)
        {
            return JsonSerializer.Serialize(o ?? new { });
        }

        public Task<string> GerarResumoAsync(string resumoBase, string habilidades, string experiencias, string formacao)
        {
            var prompt =
$@"Você é um assistente de carreira. Reescreva o resumo profissional abaixo em português formal, claro e objetivo, com foco em empregabilidade e reintegração ao mercado de trabalho.

Resumo base:
{resumoBase}

Habilidades:
{habilidades}

Experiências:
{experiencias}

Formação:
{formacao}

Regras:
- 4 a 6 linhas
- sem emojis
- sem inventar dados
- citar tecnologias e resultados quando existirem
- linguagem profissional.";

            return CallGeminiAsync(prompt);
        }

        public async Task<string[]> SugerirHabilidadesAsync(string resumoBase, string experiencias, string formacao)
        {
            var prompt =
$@"Você é um recrutador técnico. Sugira uma lista de 8 a 12 habilidades coerentes com as informações abaixo.

Resumo:
{resumoBase}

Experiências:
{experiencias}

Formação:
{formacao}

Formato de resposta:
- retorne apenas uma lista separada por vírgula
- sem texto extra
- sem emojis
- não invente experiências.";

            var result = await CallGeminiAsync(prompt);

            return result
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToArray();
        }

        public async Task<AiCurriculoFeedbackResponseDto> GerarFeedbackAsync(AiCurriculoFeedbackRequestDto req)
        {
            var cursos = await _db.Cursos
                .Select(c => new
                {
                    idCurso = c.IdCurso,
                    nomeCurso = c.NomeCurso,
                    areaCurso = c.AreaCurso,
                    descCurso = c.DescCurso,
                    linkCurso = c.LinkCurso
                })
                .ToListAsync();

            var cursosJson = JsonSerializer.Serialize(cursos);

            var prompt =
$@"Você é um especialista em reintegração ao mercado de trabalho e ATS (sistemas de triagem de currículo).
Analise o currículo abaixo e devolva SOMENTE um JSON válido seguindo exatamente este schema:

{{
  ""score"": number (0-100),
  ""summarySuggested"": string|null,
  ""gaps"": string[],
  ""suggestedBullets"": [
    {{
      ""section"": ""experiences""|""education""|""projects""|""certs"",
      ""index"": number,
      ""bullets"": string[]
    }}
  ],
  ""recommendedCourses"": [
    {{
      ""idCurso"": number,
      ""reason"": string
    }}
  ],
  ""interviewPrep"": {{
    ""questions"": string[],
    ""answersDraft"": string[]
  }}|null,
  ""raw"": string|null
}}

Regras obrigatórias:
- Responda só com JSON. Sem markdown, sem texto fora do JSON.
- Não invente dados pessoais, datas, empresas ou certificados.
- Se faltar informação, explique no campo gaps.
- score deve refletir empregabilidade real e completude.
- summarySuggested deve reescrever o resumo do usuário com foco profissional e clareza.
- suggestedBullets deve criar bullet points fortes de impacto para cada experiência/projeto existente.
- recommendedCourses: escolha de 1 a 4 cursos do banco abaixo que realmente ajudem nos gaps. Use apenas idCurso existentes.
- interviewPrep: 3 a 5 perguntas prováveis de entrevista com rascunhos curtos de resposta baseados no currículo.
- raw deve conter a justificativa breve da nota em 2-3 linhas.

Currículo do usuário (JSON):
{ToJsonSafe(req)}

Cursos disponíveis no banco (JSON):
{cursosJson}";

            var aiText = await CallGeminiAsync(prompt);

            try
            {
                var cleaned = aiText
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();


                var parsed = JsonSerializer.Deserialize<AiCurriculoFeedbackResponseDto>(cleaned, JsonOpts);

                if (parsed == null)
                    throw new Exception("Resposta nula");

                if (parsed.Score < 0) parsed.Score = 0;
                if (parsed.Score > 100) parsed.Score = 100;

                parsed.Gaps ??= new();
                parsed.SuggestedBullets ??= new();
                parsed.RecommendedCourses ??= new();

                return parsed;
            }
            catch
            {
                return new AiCurriculoFeedbackResponseDto
                {
                    Score = Math.Max(0, Math.Min(100, req.CompletenessApp)),
                    SummarySuggested = null,
                    Gaps = new() { "A IA não retornou um JSON válido. Tente novamente." },
                    SuggestedBullets = new(),
                    RecommendedCourses = new(),
                    InterviewPrep = null,
                    Raw = aiText
                };
            }
        }
    }
}
