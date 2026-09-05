using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atendimento.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaConvidado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Convidados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ChamadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convidados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Convidados_Chamados_ChamadoId",
                        column: x => x.ChamadoId,
                        principalTable: "Chamados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Convidados_ChamadoId",
                table: "Convidados",
                column: "ChamadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Convidados_Email",
                table: "Convidados",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Convidados");
        }
    }
}
