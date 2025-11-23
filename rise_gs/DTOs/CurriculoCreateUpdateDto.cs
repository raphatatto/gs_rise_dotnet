namespace rise_gs.DTOs
{
    public class CurriculoCreateUpdateDto
    {
        public string? TituloCurriculo { get; set; }
        public string? ExperienciaProfissional { get; set; }
        public string Habilidades { get; set; } = "[]";
        public string? Formacao { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public string? Projetos { get; set; }
        public string? Links { get; set; }
        public int IdUsuario { get; set; }
    }
}
