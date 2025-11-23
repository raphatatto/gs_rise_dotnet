using System.ComponentModel.DataAnnotations;

namespace rise_gs.DTOs
{
    public class UsuarioUpdateDto
    {
        [Required]
        public string NomeUsuario { get; set; } = null!;

        [Required, EmailAddress]
        public string EmailUsuario { get; set; } = null!;

        public string? SenhaUsuario { get; set; }
        public string? TipoUsuario { get; set; }
        public string? Telefone { get; set; }
        public string? Descricao { get; set; }
        public string? Habilidades { get; set; }
    }
}
