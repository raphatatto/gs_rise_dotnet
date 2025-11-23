using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rise_gs.Models
{
    public class Curriculo
    {
        public int IdCurriculo { get; set; }

        [MaxLength(200)]
        public string? TituloCurriculo { get; set; }

        [Column(TypeName = "VARCHAR2(4000)")]
        public string? ExperienciaProfissional { get; set; }

        [Column(TypeName = "VARCHAR2(4000)")]
        public string Habilidades { get; set; } = null!;

        [Column(TypeName = "VARCHAR2(4000)")]
        public string? Formacao { get; set; }

        public DateTime? UltimaAtualizacao { get; set; }

        [Column(TypeName = "VARCHAR2(4000)")]
        public string? Projetos { get; set; }

        [Column(TypeName = "VARCHAR2(4000)")]
        public string? Links { get; set; }

        [Column(TypeName = "CLOB")]
        public string? FeedbackAi { get; set; }

        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
