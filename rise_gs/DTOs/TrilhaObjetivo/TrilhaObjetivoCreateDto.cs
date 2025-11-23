namespace rise_gs.DTOs.TrilhaObjetivo
{
    public class TrilhaObjetivoCreateDto
    {
        public int IdTrilha { get; set; }             // FK para a trilha de progresso
        public string TituloObjetivo { get; set; } = null!;
        public string? CategoriaObjetivo { get; set; }
        public DateTime? DataPlanejada { get; set; }
        public bool Concluido { get; set; } = false; 
        
        public int IdUsuario { get; set; }
        }
}
