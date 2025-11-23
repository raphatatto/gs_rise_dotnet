using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.DTOs;
using rise_gs.Models;
using rise_gs.Services;
using System.Text.Json;

namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AiCurriculoController : ControllerBase
    {
        private readonly IAiCurriculoService _ai;
        private readonly RiseContext _db;

        public AiCurriculoController(IAiCurriculoService ai, RiseContext db)
        {
            _ai = ai;
            _db = db;
        }

        [HttpGet("feedback/{idUsuario:int}")]
        public async Task<IActionResult> GetFeedback(int idUsuario)
        {
            var curriculo = await _db.Curriculos
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario);

            if (curriculo == null || string.IsNullOrWhiteSpace(curriculo.FeedbackAi))
                return NotFound();

            try
            {
                var parsed = JsonSerializer.Deserialize<AiCurriculoFeedbackResponseDto>(
                    curriculo.FeedbackAi,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return Ok(parsed);
            }
            catch
            {
                return Ok(new AiCurriculoFeedbackResponseDto
                {
                    Score = 0,
                    SummarySuggested = null,
                    Gaps = new() { "Snapshot salvo está inválido." },
                    SuggestedBullets = new(),
                    RecommendedCourses = new(),
                    InterviewPrep = null,
                    Raw = curriculo.FeedbackAi
                });
            }
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> GerarFeedback([FromBody] AiCurriculoFeedbackRequestDto req)
        {
            var feedback = await _ai.GerarFeedbackAsync(req);

            var curriculo = await _db.Curriculos
                .FirstOrDefaultAsync(c => c.IdUsuario == req.IdUsuario);

            if (curriculo == null)
            {
                return BadRequest("Currículo não encontrado para o usuário.");
            }

            curriculo.FeedbackAi = JsonSerializer.Serialize(feedback);
            curriculo.UltimaAtualizacao = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(feedback);
        }
    }
}
