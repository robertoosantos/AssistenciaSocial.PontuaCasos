using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class AjusteRelacionamentoMembrosOrganizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembrosOrganizacao_Organizacoes_OrganizacaoId",
                table: "MembrosOrganizacao");

            migrationBuilder.DropForeignKey(
                name: "FK_MembrosOrganizacao_Usuarios_UsuarioId",
                table: "MembrosOrganizacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembrosOrganizacao",
                table: "MembrosOrganizacao");

            migrationBuilder.DropIndex(
                name: "IX_MembrosOrganizacao_UsuarioId",
                table: "MembrosOrganizacao");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "MembrosOrganizacao",
                newName: "MembrosId");

            migrationBuilder.RenameColumn(
                name: "OrganizacaoId",
                table: "MembrosOrganizacao",
                newName: "OrganizacoesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembrosOrganizacao",
                table: "MembrosOrganizacao",
                columns: new[] { "MembrosId", "OrganizacoesId" });

            migrationBuilder.CreateIndex(
                name: "IX_MembrosOrganizacao_OrganizacoesId",
                table: "MembrosOrganizacao",
                column: "OrganizacoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembrosOrganizacao_AspNetUsers_MembrosId",
                table: "MembrosOrganizacao",
                column: "MembrosId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MembrosOrganizacao_Organizacoes_OrganizacoesId",
                table: "MembrosOrganizacao",
                column: "OrganizacoesId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembrosOrganizacao_AspNetUsers_MembrosId",
                table: "MembrosOrganizacao");

            migrationBuilder.DropForeignKey(
                name: "FK_MembrosOrganizacao_Organizacoes_OrganizacoesId",
                table: "MembrosOrganizacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembrosOrganizacao",
                table: "MembrosOrganizacao");

            migrationBuilder.DropIndex(
                name: "IX_MembrosOrganizacao_OrganizacoesId",
                table: "MembrosOrganizacao");

            migrationBuilder.RenameColumn(
                name: "OrganizacoesId",
                table: "MembrosOrganizacao",
                newName: "OrganizacaoId");

            migrationBuilder.RenameColumn(
                name: "MembrosId",
                table: "MembrosOrganizacao",
                newName: "UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembrosOrganizacao",
                table: "MembrosOrganizacao",
                columns: new[] { "OrganizacaoId", "UsuarioId" });

            migrationBuilder.CreateIndex(
                name: "IX_MembrosOrganizacao_UsuarioId",
                table: "MembrosOrganizacao",
                column: "UsuarioId");

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
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
