using System.ComponentModel.DataAnnotations;

namespace rise_gs.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string EmailUsuario { get; set; } = string.Empty;

        [Required]
        public string SenhaUsuario { get; set; } = string.Empty;
    }
}
