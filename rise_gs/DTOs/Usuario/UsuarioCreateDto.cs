namespace rise_gs.DTOs
{
    public class UsuarioCreateDto
    {
        public string NomeUsuario { get; set; } = null!;
        public string? EmailUsuario { get; set; }
        public string SenhaUsuario { get; set; } = null!;
        public string? TipoUsuario { get; set; }

        public string? Telefone { get; set; }
        public string? Descricao { get; set; }
        public string? Habilidades { get; set; }
    }
}
