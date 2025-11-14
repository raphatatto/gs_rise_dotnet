using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rise_gs.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_RISE_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME__USUARIO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    EMAIL_USUARIO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    SENHA_USUARIO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    TIPO_USUARIO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISE_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_RISE_BEM_ESTAR",
                columns: table => new
                {
                    ID__BEM_ESTAR = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DT_REGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NIVEL_HUMOR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    HORAS_ESTUDO = table.Column<TimeSpan>(type: "INTERVAL DAY(8) TO SECOND(7)", nullable: true),
                    DESC_ATIVIDADE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISE_BEM_ESTAR", x => x.ID__BEM_ESTAR);
                    table.ForeignKey(
                        name: "FK_RISE_BEM_ESTAR_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_RISE_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_RISE_CURRICULO",
                columns: table => new
                {
                    ID_CURRICULO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    HABILIDADES = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    TITULO_CURRICULO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    EXPERIENCIA_PROFISSIONAL = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    FORMACAO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    ULTIMA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PROJETOS = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    LINKS = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISE_CURRICULO", x => new { x.ID_CURRICULO, x.HABILIDADES });
                    table.ForeignKey(
                        name: "FK_RISE_CURRICULO_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_RISE_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_RISE_CURSO",
                columns: table => new
                {
                    ID_CURSO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_CURSO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DESC_CURSO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    LINK_CURSO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    AREA_CURSO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISE_CURSO", x => x.ID_CURSO);
                    table.ForeignKey(
                        name: "FK_RISE_CURSO_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_RISE_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_RISE_TRILHA_PROGRESSO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PERCENTUAL_CONCLUIDO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DT_INICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DT_ULTIMA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISE_TRILHA_PROGRESSO", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_RISE_PROGRESSO_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_RISE_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_RISE_BEM_ESTAR_ID_USUARIO",
                table: "TB_RISE_BEM_ESTAR",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_RISE_CURRICULO_ID_USUARIO",
                table: "TB_RISE_CURRICULO",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_RISE_CURSO_ID_USUARIO",
                table: "TB_RISE_CURSO",
                column: "ID_USUARIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_RISE_BEM_ESTAR");

            migrationBuilder.DropTable(
                name: "TB_RISE_CURRICULO");

            migrationBuilder.DropTable(
                name: "TB_RISE_CURSO");

            migrationBuilder.DropTable(
                name: "TB_RISE_TRILHA_PROGRESSO");

            migrationBuilder.DropTable(
                name: "TB_RISE_USUARIO");
        }
    }
}
