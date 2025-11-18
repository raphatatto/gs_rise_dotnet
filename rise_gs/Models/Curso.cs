namespace rise_gs.Models
{
    public class Curso
    {
        public int IdCurso { get; set; }
        public string? NomeCurso { get; set; }
        public string? DescCurso { get; set; }
        public string? LinkCurso { get; set; }
        public string? AreaCurso { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
