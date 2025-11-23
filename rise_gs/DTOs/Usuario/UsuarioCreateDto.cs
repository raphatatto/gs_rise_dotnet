using System.ComponentModel.DataAnnotations;

namespace rise_gs.DTOs
{
    public class UsuarioCreateDto
    {
        [Required]
        public string NomeUsuario { get; set; } = null!;

        [Required, EmailAddress]
        public string EmailUsuario { get; set; } = null!;

        [Required]
        public string SenhaUsuario { get; set; } = null!;

        public string? TipoUsuario { get; set; }

        public string? Telefone { get; set; }
        public string? Descricao { get; set; }

        public string? Habilidades { get; set; }
    }
}
