using System;

namespace rise_gs.DTOs
{
    public class TrilhaObjetivoCreateDto
    {
        public int IdUsuario { get; set; }
        public string TituloObjetivo { get; set; } = null!;
        public string? CategoriaObjetivo { get; set; }
        public DateTime? DataPlanejada { get; set; }
    }
}
