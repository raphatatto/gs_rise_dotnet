namespace rise_gs.DTOs.Usuario
{
    public class LoginRequestDto
    {
        public string EmailUsuario { get; set; } = string.Empty;
        public string SenhaUsuario { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
        public int IdUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public string? TipoUsuario { get; set; }
    }
}
