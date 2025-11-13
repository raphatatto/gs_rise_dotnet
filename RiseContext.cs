using Microsoft.EntityFrameworkCore;
using rise_gs.Models;

namespace rise_gs
{
    public class RiseContext : DbContext
    {
        public RiseContext(DbContextOptions<RiseContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<BemEstar> BemEstares => Set<BemEstar>();
        public DbSet<Curriculo> Curriculos => Set<Curriculo>();
        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<TrilhaProgresso> TrilhasProgresso => Set<TrilhaProgresso>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // USUARIO  ------------------------------------------------------
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("TB_RISE_USUARIO");

                entity.HasKey(e => e.IdUsuario)
                      .HasName("PK_RISE_USUARIO");

                entity.Property(e => e.IdUsuario)
                      .HasColumnName("ID_USUARIO");

                entity.Property(e => e.NomeUsuario)
                      .HasColumnName("NOME__USUARIO")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.EmailUsuario)
                      .HasColumnName("EMAIL_USUARIO")
                      .HasMaxLength(50);

                entity.Property(e => e.SenhaUsuario)
                      .HasColumnName("SENHA_USUARIO")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.TipoUsuario)
                      .HasColumnName("TIPO_USUARIO")
                      .HasMaxLength(50);
            });

            // BEM-ESTAR  ----------------------------------------------------
            modelBuilder.Entity<BemEstar>(entity =>
            {
                entity.ToTable("TB_RISE_BEM_ESTAR");

                entity.HasKey(e => e.IdBemEstar)
                      .HasName("PK_RISE_BEM_ESTAR");

                entity.Property(e => e.IdBemEstar)
                      .HasColumnName("ID__BEM_ESTAR");

                entity.Property(e => e.DtRegistro)
                      .HasColumnName("DT_REGISTRO");

                entity.Property(e => e.NivelHumor)
                      .HasColumnName("NIVEL_HUMOR");

                entity.Property(e => e.HorasEstudo)
                      .HasColumnName("HORAS_ESTUDO");

                entity.Property(e => e.DescAtividade)
                      .HasColumnName("DESC_ATIVIDADE")
                      .HasMaxLength(50);

                entity.Property(e => e.IdUsuario)
                      .HasColumnName("ID_USUARIO");

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.RegistrosBemEstar)
                      .HasForeignKey(e => e.IdUsuario)
                      .HasConstraintName("FK_RISE_BEM_ESTAR_USUARIO");
            });

            // CURRÍCULO  ----------------------------------------------------
            modelBuilder.Entity<Curriculo>(entity =>
            {
                entity.ToTable("TB_RISE_CURRICULO");

                // PK composta: (id_curriculo, habilidades)
                entity.HasKey(e => new { e.IdCurriculo, e.Habilidades })
                      .HasName("PK_RISE_CURRICULO");

                entity.Property(e => e.IdCurriculo)
                      .HasColumnName("ID_CURRICULO");

                entity.Property(e => e.TituloCurriculo)
                      .HasColumnName("TITULO_CURRICULO")
                      .HasMaxLength(100);

                entity.Property(e => e.ExperienciaProfissional)
                      .HasColumnName("EXPERIENCIA_PROFISSIONAL")
                      .HasMaxLength(1000);

                entity.Property(e => e.Habilidades)
                      .HasColumnName("HABILIDADES")
                      .HasMaxLength(1000)
                      .IsRequired();

                entity.Property(e => e.Formacao)
                      .HasColumnName("FORMACAO")
                      .HasMaxLength(1000);

                entity.Property(e => e.UltimaAtualizacao)
                      .HasColumnName("ULTIMA_ATUALIZACAO");

                entity.Property(e => e.Projetos)
                      .HasColumnName("PROJETOS")
                      .HasMaxLength(100);

                entity.Property(e => e.Links)
                      .HasColumnName("LINKS")
                      .HasMaxLength(100);

                entity.Property(e => e.IdUsuario)
                      .HasColumnName("ID_USUARIO");

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Curriculos)
                      .HasForeignKey(e => e.IdUsuario)
                      .HasConstraintName("FK_RISE_CURRICULO_USUARIO");
            });

            // CURSO  --------------------------------------------------------
            modelBuilder.Entity<Curso>(entity =>
            {
                entity.ToTable("TB_RISE_CURSO");

                entity.HasKey(e => e.IdCurso)
                      .HasName("PK_RISE_CURSO");

                entity.Property(e => e.IdCurso)
                      .HasColumnName("ID_CURSO");

                entity.Property(e => e.NomeCurso)
                      .HasColumnName("NOME_CURSO")
                      .HasMaxLength(50);

                entity.Property(e => e.DescCurso)
                      .HasColumnName("DESC_CURSO")
                      .HasMaxLength(50);

                entity.Property(e => e.LinkCurso)
                      .HasColumnName("LINK_CURSO")
                      .HasMaxLength(150);

                entity.Property(e => e.AreaCurso)
                      .HasColumnName("AREA_CURSO")
                      .HasMaxLength(50);

                entity.Property(e => e.IdUsuario)
                      .HasColumnName("ID_USUARIO");

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Cursos)
                      .HasForeignKey(e => e.IdUsuario)
                      .HasConstraintName("FK_RISE_CURSO_USUARIO");
            });

            // TRILHA DE PROGRESSO  -----------------------------------------
            modelBuilder.Entity<TrilhaProgresso>(entity =>
            {
                entity.ToTable("TB_RISE_TRILHA_PROGRESSO");

                // vamos usar ID_USUARIO como PK (1:1 com Usuario)
                entity.HasKey(e => e.IdUsuario)
                      .HasName("PK_RISE_TRILHA_PROGRESSO");

                entity.Property(e => e.IdUsuario)
                      .HasColumnName("ID_USUARIO");

                entity.Property(e => e.PercentualConcluido)
                      .HasColumnName("PERCENTUAL_CONCLUIDO");

                entity.Property(e => e.DtInicio)
                      .HasColumnName("DT_INICIO");

                entity.Property(e => e.DtUltimaAtualizacao)
                      .HasColumnName("DT_ULTIMA_ATUALIZACAO");

                entity.HasOne(e => e.Usuario)
                      .WithOne(u => u.TrilhaProgresso)
                      .HasForeignKey<TrilhaProgresso>(e => e.IdUsuario)
                      .HasConstraintName("FK_RISE_PROGRESSO_USUARIO");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
