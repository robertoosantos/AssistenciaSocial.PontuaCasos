using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class AjusteRelacionamentoItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_AspNetUsers_ModificadoPorId",
                table: "Itens");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_AspNetUsers_ModificadoPorId",
                table: "Itens",
                column: "ModificadoPorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_AspNetUsers_ModificadoPorId",
                table: "Itens");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_AspNetUsers_ModificadoPorId",
                table: "Itens",
                column: "ModificadoPorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
