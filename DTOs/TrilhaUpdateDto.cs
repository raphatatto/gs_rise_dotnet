namespace rise_gs.DTOs
{
    public class TrilhaUpdateDto
    {
        public int IdUsuario { get; set; }
        public int? PercentualConcluido { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtUltimaAtualizacao { get; set; }
    }
}
