
namespace rise_gs.DTOs.TrilhaObjetivo
{
	public class TrilhaObjetivoUpdateDto
	{
		public string? TituloObjetivo { get; set; }
		public string? CategoriaObjetivo { get; set; }
		public DateTime? DataPlanejada { get; set; }
		public string? Concluido { get; set; }
		public DateTime? DataConclusao { get; set; }
	}
}
