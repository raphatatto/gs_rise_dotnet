namespace rise_gs.DTOs
{
    public class UsuarioCreateDto
    {
        public string NomeUsuario { get; set; } = null!;
        public string? EmailUsuario { get; set; }
        public string SenhaUsuario { get; set; } = null!;
        public string? TipoUsuario { get; set; }
    }
}
