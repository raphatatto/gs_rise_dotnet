using System.Threading.Tasks;
using rise_gs.DTOs;

namespace rise_gs.Services
{
    public interface IAiCurriculoService
    {
        Task<string> GerarResumoAsync(string resumoBase, string habilidades, string experiencias, string formacao);
        Task<string[]> SugerirHabilidadesAsync(string resumoBase, string experiencias, string formacao);
        Task<AiCurriculoFeedbackResponseDto> GerarFeedbackAsync(AiCurriculoFeedbackRequestDto req);
    }
}
