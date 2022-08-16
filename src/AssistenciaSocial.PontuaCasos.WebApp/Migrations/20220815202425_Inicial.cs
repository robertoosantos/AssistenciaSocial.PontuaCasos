using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Multiplo = table.Column<bool>(type: "bit", nullable: false),
                    OrganizacaoId = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CriadoPorId = table.Column<int>(type: "int", nullable: false),
                    ModificadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoPorId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itens_Itens_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Itens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MembrosOrganizacao",
                columns: table => new
                {
                    OrganizacaoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembrosOrganizacao", x => new { x.OrganizacaoId, x.UsuarioId });
                });

            migrationBuilder.CreateTable(
                name: "Organizacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CriadoPorId = table.Column<int>(type: "int", nullable: false),
                    ModificadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoPorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CriadoPorId = table.Column<int>(type: "int", nullable: true),
                    ModificadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoPorId = table.Column<int>(type: "int", nullable: true),
                    OrganizacaoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuarios_Usuarios_CriadoPorId",
                        column: x => x.CriadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuarios_Usuarios_ModificadoPorId",
                        column: x => x.ModificadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itens_CriadoPorId",
                table: "Itens",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_ItemId",
                table: "Itens",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_ModificadoPorId",
                table: "Itens",
                column: "ModificadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Itens_OrganizacaoId",
                table: "Itens",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_MembrosOrganizacao_UsuarioId",
                table: "MembrosOrganizacao",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizacoes_CriadoPorId",
                table: "Organizacoes",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizacoes_ModificadoPorId",
                table: "Organizacoes",
                column: "ModificadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CriadoPorId",
                table: "Usuarios",
                column: "CriadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ModificadoPorId",
                table: "Usuarios",
                column: "ModificadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_OrganizacaoId",
                table: "Usuarios",
                column: "OrganizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Organizacoes_OrganizacaoId",
                table: "Itens",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Usuarios_CriadoPorId",
                table: "Itens",
                column: "CriadoPorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Usuarios_ModificadoPorId",
                table: "Itens",
                column: "ModificadoPorId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembrosOrganizacao_Organizacoes_OrganizacaoId",
                table: "MembrosOrganizacao",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembrosOrganizacao_Usuarios_UsuarioId",
                table: "MembrosOrganizacao",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizacoes_Usuarios_CriadoPorId",
                table: "Organizacoes",
                column: "CriadoPorId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizacoes_Usuarios_ModificadoPorId",
                table: "Organizacoes",
                column: "ModificadoPorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Organizacoes_OrganizacaoId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "MembrosOrganizacao");

            migrationBuilder.DropTable(
                name: "Organizacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
