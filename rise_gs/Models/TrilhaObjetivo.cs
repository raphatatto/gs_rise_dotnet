namespace rise_gs.Models
{
    public class TrilhaObjetivo
    {
        public int IdObjetivo { get; set; }              
        public string? TituloObjetivo { get; set; }
        public string? CategoriaObjetivo { get; set; }
        public DateTime? DataPlanejada { get; set; }
        public string? Concluido { get; set; }           
        public DateTime? DataConclusao { get; set; }
        public DateTime? DtCriacao { get; set; }
        public int IdTrilha { get; set; }                
        public TrilhaProgresso Trilha { get; set; } = null!;
    }
}
