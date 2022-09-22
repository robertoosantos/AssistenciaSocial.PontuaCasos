using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class RemocaoPontosCasos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViolenciasSofridas_IndividuosEmViolacoes_IndividuoEmViolacaoId",
                table: "ViolenciasSofridas");

            migrationBuilder.DropForeignKey(
                name: "FK_ViolenciasSofridas_Itens_SituacaoId",
                table: "ViolenciasSofridas");

            migrationBuilder.DropIndex(
                name: "IX_ViolenciasSofridas_IndividuoEmViolacaoId",
                table: "ViolenciasSofridas");

            migrationBuilder.DropColumn(
                name: "Pontos",
                table: "Casos");

            migrationBuilder.CreateIndex(
                name: "IX_ViolenciasSofridas_IndividuoEmViolacaoId_ViolenciaId",
                table: "ViolenciasSofridas",
                columns: new[] { "IndividuoEmViolacaoId", "ViolenciaId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ViolenciasSofridas_IndividuosEmViolacoes_IndividuoEmViolacaoId",
                table: "ViolenciasSofridas",
                column: "IndividuoEmViolacaoId",
                principalTable: "IndividuosEmViolacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ViolenciasSofridas_Itens_SituacaoId",
                table: "ViolenciasSofridas",
                column: "SituacaoId",
                principalTable: "Itens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViolenciasSofridas_IndividuosEmViolacoes_IndividuoEmViolacaoId",
                table: "ViolenciasSofridas");

            migrationBuilder.DropForeignKey(
                name: "FK_ViolenciasSofridas_Itens_SituacaoId",
                table: "ViolenciasSofridas");

            migrationBuilder.DropIndex(
                name: "IX_ViolenciasSofridas_IndividuoEmViolacaoId_ViolenciaId",
                table: "ViolenciasSofridas");

            migrationBuilder.AddColumn<int>(
                name: "Pontos",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ViolenciasSofridas_IndividuoEmViolacaoId",
                table: "ViolenciasSofridas",
                column: "IndividuoEmViolacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ViolenciasSofridas_IndividuosEmViolacoes_IndividuoEmViolacaoId",
                table: "ViolenciasSofridas",
                column: "IndividuoEmViolacaoId",
                principalTable: "IndividuosEmViolacoes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ViolenciasSofridas_Itens_SituacaoId",
                table: "ViolenciasSofridas",
                column: "SituacaoId",
                principalTable: "Itens",
                principalColumn: "Id");
        }
    }
}
