namespace rise_gs.DTOs
{
    public class UsuarioDto
    {
       public int IdUsuario { get; set; }
       public string NomeUsuario { get; set; } = null!;
       
       public string EmailUsuario { get; set; } = null!;
       public string? TipoUsuario { get; set; }
       public List<LinkDto> Links { get; set; } = new();
    }
}
