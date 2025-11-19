namespace rise_gs.DTOs
{
    public class TrilhaProgressoDto
    {
        public int IdTrilha { get; set; }
        public int IdUsuario { get; set; }
        public int? PercentualConcluido { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtUltimaAtualizacao { get; set; }
        public string? TituloTrilha { get; set; }
        public string? CategoriaTrilha { get; set; }
        public DateTime? DataPlanejada { get; set; }
        public DateTime? DtCriacao { get; set; }
    }

    public class TrilhaProgressoCreateDto
    {
        public int IdUsuario { get; set; }
        public string TituloTrilha { get; set; } = null!;
        public string? CategoriaTrilha { get; set; }
        public DateTime? DataPlanejada { get; set; }
    }

    public class TrilhaProgressoUpdateDto
    {
        public int? PercentualConcluido { get; set; }
        public string? TituloTrilha { get; set; }
        public string? CategoriaTrilha { get; set; }
        public DateTime? DataPlanejada { get; set; }
    }
}
