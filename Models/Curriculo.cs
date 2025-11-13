namespace rise_gs.Models
{
    public class Curriculo
    {
        public int IdCurriculo { get; set; }
        public string? TituloCurriculo { get; set; }
        public string? ExperienciaProfissional { get; set; }
        public string Habilidades { get; set; } = null!;

        public string? Formacao { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public string? Projetos { get; set; }
        public string? Links { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
