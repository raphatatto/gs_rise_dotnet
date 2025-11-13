namespace rise_gs.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string SenhaUsuario { get; set; }
        public string? TipoUsuario { get; set; }

        public ICollection<BemEstar> RegistrosBemEstar { get; set; } = new List<BemEstar>();
        public ICollection<Curriculo> Curriculos { get; set; } = new List<Curriculo>();
        public ICollection<Curso> Cursos { get; set; } 
        public TrilhaProgresso? TrilhaProgresso { get; set; }
    }
}
