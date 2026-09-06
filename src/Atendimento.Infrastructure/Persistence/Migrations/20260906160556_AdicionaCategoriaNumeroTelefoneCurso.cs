using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atendimento.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCategoriaNumeroTelefoneCurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ChamadoNumeroSequence",
                startValue: 1000L);

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Chamados",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "Chamados",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR ChamadoNumeroSequence");

            migrationBuilder.AddColumn<string>(
                name: "Curso",
                table: "Alunos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Alunos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_Numero",
                table: "Chamados",
                column: "Numero",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chamados_Numero",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Curso",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Alunos");

            migrationBuilder.DropSequence(
                name: "ChamadoNumeroSequence");
        }
    }
}
