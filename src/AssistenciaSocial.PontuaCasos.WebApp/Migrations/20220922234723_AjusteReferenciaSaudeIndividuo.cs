using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class AjusteReferenciaSaudeIndividuo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaudeIndividuos_Individuos_IndividuoId",
                table: "SaudeIndividuos");

            migrationBuilder.AddForeignKey(
                name: "FK_SaudeIndividuos_Individuos_IndividuoId",
                table: "SaudeIndividuos",
                column: "IndividuoId",
                principalTable: "IndividuosEmViolacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaudeIndividuos_Individuos_IndividuoId",
                table: "SaudeIndividuos");

            migrationBuilder.AddForeignKey(
                name: "FK_SaudeIndividuos_Individuos_IndividuoId",
                table: "SaudeIndividuos",
                column: "IndividuoId",
                principalTable: "IndividuosEmViolacoes",
                principalColumn: "Id");
        }
    }
}
