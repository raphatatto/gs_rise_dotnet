namespace rise_gs.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string SenhaUsuario { get; set; }
        public string? TipoUsuario { get; set; }
        public string? Telefone { get; set; }
        public string? Descricao { get; set; }
        public string? Habilidades { get; set; }


        public ICollection<BemEstar> RegistrosBemEstar { get; set; } = new List<BemEstar>();
        public ICollection<Curriculo> Curriculos { get; set; } = new List<Curriculo>();
        public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
        public ICollection<TrilhaProgresso> Trilhas { get; set; } = new List<TrilhaProgresso>();
    }
}
