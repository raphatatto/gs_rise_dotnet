namespace rise_gs.Models
{
    public class TrilhaProgresso
    {
        public int IdUsuario { get; set; } 
        public Usuario Usuario { get; set; } = null!;

        public int? PercentualConcluido { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtUltimaAtualizacao { get; set; }
    }
}
