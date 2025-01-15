using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistenciaSocial.PontuaCasos.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeExportacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                EXEC (N'
                    CREATE OR ALTER PROCEDURE ExportarCasos
                    AS
                    BEGIN
                        DECLARE @colunas NVARCHAR(MAX);
                        DECLARE @sql NVARCHAR(MAX);

                        SELECT @colunas = STRING_AGG(QUOTENAME(Titulo), '','')
                        FROM (SELECT DISTINCT Titulo
                            FROM Itens
                            WHERE ECategoria = 0 
                            AND Ativo = 1) AS DistinctTitles;

                        SET @sql = N''
                        SELECT
                            c.ResponsavelFamiliar,
                            c.Titulo,
                            c.Prontuario,
                            p.*
                        FROM
                            Casos c
                            INNER JOIN
                            (
                        SELECT *
                            FROM (
                                    SELECT
                                        i.Titulo,
                                        i.Pontos * c.Pontos Pontos,
                                        iev.CasoId
                                    FROM IndividuosEmViolacoes iev
                                        INNER JOIN Itens i
                                        ON i.Id = iev.ItemId
                                        INNER JOIN Itens c
                                        ON c.Id = i.CategoriaId

                                UNION ALL

                                    SELECT
                                        i.Titulo,
                                        i.Pontos * c.Pontos Pontos,
                                        ifa.CasoId
                                    FROM ItensFamiliares ifa
                                        INNER JOIN Itens i
                                        ON i.Id = ifa.ItemFamiliarId
                                        INNER JOIN Itens c
                                        ON c.Id = i.CategoriaId

                                UNION ALL

                                    SELECT
                                        i.Titulo,
                                        i.Pontos * c.Pontos Pontos,
                                        iev.CasoId
                                    FROM SaudeIndividuos si
                                        INNER JOIN IndividuosEmViolacoes iev
                                        ON iev.Id = si.IndividuoId
                                        INNER JOIN Itens i
                                        ON i.Id = si.ItemSaudeId
                                        INNER JOIN Itens c
                                        ON c.Id = i.CategoriaId

                                UNION ALL

                                    SELECT
                                        i.Titulo,
                                        i.Pontos * c.Pontos Pontos,
                                        iev.CasoId
                                    FROM ViolenciasSofridas vs
                                        INNER JOIN IndividuosEmViolacoes iev
                                        ON iev.Id = vs.IndividuoEmViolacaoId
                                        INNER JOIN Itens i
                                        ON i.Id = vs.SituacaoId
                                        INNER JOIN Itens c
                                        ON c.Id = i.CategoriaId

                                UNION ALL

                                    SELECT
                                        i.Titulo,
                                        i.Pontos * c.Pontos Pontos,
                                        iev.CasoId
                                    FROM ViolenciasSofridas vs
                                        INNER JOIN IndividuosEmViolacoes iev
                                        ON iev.Id = vs.IndividuoEmViolacaoId
                                        INNER JOIN Itens i
                                        ON i.Id = vs.ViolenciaId
                                        INNER JOIN Itens c
                                        ON c.Id = i.CategoriaId
                        ) AS SourceTable
                        PIVOT (
                            SUM(Pontos)
                            FOR Titulo IN ('' + @colunas + '')
                        ) AS PivotTable) p
                            ON p.CasoId = c.Id
                        WHERE
                            Ativo = 1
                        '';

                        EXEC sp_executesql @sql;
                    END
                ')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE ExportarCasos");
        }
    }
}
