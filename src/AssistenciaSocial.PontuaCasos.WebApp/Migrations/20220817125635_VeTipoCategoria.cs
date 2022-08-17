using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class VeTipoCategoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Categoria",
                table: "Itens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Itens");
        }
    }
}
