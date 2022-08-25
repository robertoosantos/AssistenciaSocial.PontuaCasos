using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class NovaModelagemItens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Itens_ItemId",
                table: "Itens");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensCasos_ItensCasos_ItemPai",
                table: "ItensCasos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensCasos",
                table: "ItensCasos");

            migrationBuilder.DropIndex(
                name: "IX_ItensCasos_CasoId",
                table: "ItensCasos");

            migrationBuilder.DropIndex(
                name: "IX_ItensCasos_ItemPai",
                table: "ItensCasos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ItensCasos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "ItensCasos");

            migrationBuilder.DropColumn(
                name: "ItemPai",
                table: "ItensCasos");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Itens",
                newName: "CategoriaId");

            migrationBuilder.RenameColumn(
                name: "Categoria",
                table: "Itens",
                newName: "ECategoria");

            migrationBuilder.RenameIndex(
                name: "IX_Itens_ItemId",
                table: "Itens",
                newName: "IX_Itens_CategoriaId");

            migrationBuilder.AddColumn<int>(
                name: "RelacionadoAoId",
                table: "Itens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prontuario",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponsavelFamiliar",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensCasos",
                table: "ItensCasos",
                columns: new[] { "CasoId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_Itens_RelacionadoAoId",
                table: "Itens",
                column: "RelacionadoAoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Itens_CategoriaId",
                table: "Itens",
                column: "CategoriaId",
                principalTable: "Itens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Itens_RelacionadoAoId",
                table: "Itens",
                column: "RelacionadoAoId",
                principalTable: "Itens",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Itens_CategoriaId",
                table: "Itens");

            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Itens_RelacionadoAoId",
                table: "Itens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensCasos",
                table: "ItensCasos");

            migrationBuilder.DropIndex(
                name: "IX_Itens_RelacionadoAoId",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "RelacionadoAoId",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "Prontuario",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "ResponsavelFamiliar",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Casos");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "Itens",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "ECategoria",
                table: "Itens",
                newName: "Categoria");

            migrationBuilder.RenameIndex(
                name: "IX_Itens_CategoriaId",
                table: "Itens",
                newName: "IX_Itens_ItemId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ItensCasos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "ItensCasos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ItemPai",
                table: "ItensCasos",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensCasos",
                table: "ItensCasos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCasos_CasoId",
                table: "ItensCasos",
                column: "CasoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCasos_ItemPai",
                table: "ItensCasos",
                column: "ItemPai");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Itens_ItemId",
                table: "Itens",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCasos_ItensCasos_ItemPai",
                table: "ItensCasos",
                column: "ItemPai",
                principalTable: "ItensCasos",
                principalColumn: "Id");
        }
    }
}
