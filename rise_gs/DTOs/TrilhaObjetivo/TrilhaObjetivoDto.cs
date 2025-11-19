namespace rise_gs.DTOs
{
    public class TrilhaObjetivoDto
    {
        public int IdObjetivo { get; set; }              // PK
        public int IdTrilha { get; set; }                // FK para a trilha

        public string? TituloObjetivo { get; set; }
        public string? CategoriaObjetivo { get; set; }
        public DateTime? DataPlanejada { get; set; }

        // "S" ou "N" – pode ser nulo no banco, então deixamos nullable aqui também
        public string? Concluido { get; set; }

        public DateTime? DataConclusao { get; set; }
        public DateTime? DtCriacao { get; set; }
    }
}
