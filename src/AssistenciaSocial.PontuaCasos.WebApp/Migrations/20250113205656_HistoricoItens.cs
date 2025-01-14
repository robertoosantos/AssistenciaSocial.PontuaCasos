using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    public partial class HistoricoItens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Itens")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ItensHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "Itens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "Itens",
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
                table: "Itens")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ItensHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "Itens")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ItensHistorico")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");

            migrationBuilder.AlterTable(
                name: "Itens")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "ItensHistorico")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "ValidoAte")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "ValidoDe");
        }
    }
}
