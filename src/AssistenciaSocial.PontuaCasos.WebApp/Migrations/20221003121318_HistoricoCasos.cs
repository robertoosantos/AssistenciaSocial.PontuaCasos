using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class HistoricoCasos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "ViolenciasSofridas")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ViolenciasSofridasHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "SaudeIndividuos")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SaudeIndividuosHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "ItensFamiliares")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ItensFamiliaresHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "IndividuosEmViolacoes")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "IndividuosEmViolacoesHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "Casos")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "CasosHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "ViolenciasSofridas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "ViolenciasSofridas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "SaudeIndividuos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "SaudeIndividuos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "ItensFamiliares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "ItensFamiliares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "IndividuosEmViolacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "IndividuosEmViolacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "Casos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "Casos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "ViolenciasSofridas")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ViolenciasSofridasHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "ViolenciasSofridas")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ViolenciasSofridasHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "SaudeIndividuos")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SaudeIndividuosHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "SaudeIndividuos")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SaudeIndividuosHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "ItensFamiliares")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ItensFamiliaresHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "ItensFamiliares")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ItensFamiliaresHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "IndividuosEmViolacoes")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "IndividuosEmViolacoesHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "IndividuosEmViolacoes")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "IndividuosEmViolacoesHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "Casos")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "CasosHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "Casos")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "CasosHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "ViolenciasSofridas")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "ViolenciasSofridasHistorico")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "SaudeIndividuos")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "SaudeIndividuosHistorico")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "ItensFamiliares")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "ItensFamiliaresHistorico")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "IndividuosEmViolacoes")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "IndividuosEmViolacoesHistorico")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "Casos")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "CasosHistorico")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");
        }
    }
}
