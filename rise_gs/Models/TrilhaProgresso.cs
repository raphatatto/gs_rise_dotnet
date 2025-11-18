namespace rise_gs.Models
{
    public class TrilhaProgresso
    {
        public int IdTrilha { get; set; }                  // ID_TRILHA (PK)
        public int IdUsuario { get; set; }                 // FK para usuário

        public int? PercentualConcluido { get; set; }      // NUMBER(5)
        public DateTime? DtInicio { get; set; }
        public DateTime? DtUltimaAtualizacao { get; set; }

        public string? TituloTrilha { get; set; }
        public string? CategoriaTrilha { get; set; }
        public DateTime? DataPlanejada { get; set; }
        public DateTime? DtCriacao { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public ICollection<TrilhaObjetivo> Objetivos { get; set; } = new List<TrilhaObjetivo>();
    }
}
