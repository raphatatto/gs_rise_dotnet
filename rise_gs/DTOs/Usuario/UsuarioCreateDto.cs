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
<<<<<<< HEAD:rise_gs/DTOs/Usuario/UsuarioCreateDto.cs

        public string? Telefone { get; set; }
        public string? Descricao { get; set; }
=======
        public string? Telefone { get; set; }
        public string? Descricao { get; set; }

        // no banco é string (json), então aqui também pode vir string
        // exemplo: "[\"React\",\"SQL\"]"
>>>>>>> bd27691 (adicionando IA):rise_gs/DTOs/UsuarioCreateDto.cs
        public string? Habilidades { get; set; }
    }
}
