using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class NovaModelagemCaso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Casos_Organizacoes_OrganizacaoId",
                table: "Casos");

            migrationBuilder.DropTable(
                name: "ItensCasos");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizacaoId",
                table: "Casos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "IndividuosEmViolacoes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CasoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividuosEmViolacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividuosEmViolacoes_Casos_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Casos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IndividuosEmViolacoes_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItensFamiliares",
                columns: table => new
                {
                    CasoId = table.Column<int>(type: "int", nullable: false),
                    ItemFamiliarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensFamiliares", x => new { x.CasoId, x.ItemFamiliarId });
                    table.ForeignKey(
                        name: "FK_ItensFamiliares_Casos_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Casos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItensFamiliares_Itens_ItemFamiliarId",
                        column: x => x.ItemFamiliarId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaudeIndividuos",
                columns: table => new
                {
                    IndividuoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemSaudeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaudeIndividuos", x => new { x.IndividuoId, x.ItemSaudeId });
                    table.ForeignKey(
                        name: "FK_SaudeIndividuos_Individuos_IndividuoId",
                        column: x => x.IndividuoId,
                        principalTable: "IndividuosEmViolacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaudeIndividuos_Itens_ItemSaudeId",
                        column: x => x.ItemSaudeId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViolenciasSofridas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SituacaoId = table.Column<int>(type: "int", nullable: true),
                    ViolenciaId = table.Column<int>(type: "int", nullable: false),
                    IndividuoEmViolacaoId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolenciasSofridas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViolenciasSofridas_IndividuosEmViolacoes_IndividuoEmViolacaoId",
                        column: x => x.IndividuoEmViolacaoId,
                        principalTable: "IndividuosEmViolacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ViolenciasSofridas_Itens_SituacaoId",
                        column: x => x.SituacaoId,
                        principalTable: "Itens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ViolenciasSofridas_Itens_ViolenciaId",
                        column: x => x.ViolenciaId,
                        principalTable: "Itens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndividuosEmViolacoes_CasoId",
                table: "IndividuosEmViolacoes",
                column: "CasoId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividuosEmViolacoes_ItemId",
                table: "IndividuosEmViolacoes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensFamiliares_ItemFamiliarId",
                table: "ItensFamiliares",
                column: "ItemFamiliarId");

            migrationBuilder.CreateIndex(
                name: "IX_SaudeIndividuos_ItemSaudeId",
                table: "SaudeIndividuos",
                column: "ItemSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_ViolenciasSofridas_IndividuoEmViolacaoId",
                table: "ViolenciasSofridas",
                column: "IndividuoEmViolacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ViolenciasSofridas_SituacaoId",
                table: "ViolenciasSofridas",
                column: "SituacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ViolenciasSofridas_ViolenciaId",
                table: "ViolenciasSofridas",
                column: "ViolenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Casos_Organizacoes_OrganizacaoId",
                table: "Casos",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Casos_Organizacoes_OrganizacaoId",
                table: "Casos");

            migrationBuilder.DropTable(
                name: "ItensFamiliares");

            migrationBuilder.DropTable(
                name: "SaudeIndividuos");

            migrationBuilder.DropTable(
                name: "ViolenciasSofridas");

            migrationBuilder.DropTable(
                name: "IndividuosEmViolacoes");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizacaoId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ItensCasos",
                columns: table => new
                {
                    CasoId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensCasos", x => new { x.CasoId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_ItensCasos_Casos_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Casos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItensCasos_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensCasos_ItemId",
                table: "ItensCasos",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Casos_Organizacoes_OrganizacaoId",
                table: "Casos",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
