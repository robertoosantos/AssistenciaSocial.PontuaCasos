using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class RefactoringControle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Casos_AspNetUsers_ModificadoPorId",
                table: "Casos");

            migrationBuilder.AddForeignKey(
                name: "FK_Casos_AspNetUsers_ModificadoPorId",
                table: "Casos",
                column: "ModificadoPorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Casos_AspNetUsers_ModificadoPorId",
                table: "Casos");

            migrationBuilder.AddForeignKey(
                name: "FK_Casos_AspNetUsers_ModificadoPorId",
                table: "Casos",
                column: "ModificadoPorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
