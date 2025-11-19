namespace rise_gs.DTOs
{
    public class UsuarioUpdateDto
    {
        public string NomeUsuario { get; set; } = null!;
        public string? EmailUsuario { get; set; }
        public string? SenhaUsuario { get; set; }
        public string? TipoUsuario { get; set; }

        public string? Telefone { get; set; }
        public string? Descricao { get; set; }
        public string? Habilidades { get; set; }
    }
}
