using rise_gs.Models;

namespace rise_gs.DTOs
{
    public class TrilhaCreateDto
    {
        public int IdUsuario { get; set; }
        public int? PercentualConcluido { get; set; }
        public DateTime? DtInicio { get; set; }
    }
}
