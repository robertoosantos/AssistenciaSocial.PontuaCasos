using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Data.Common;
using System.Data;

namespace AssistenciaSocial.PontuaCasos.WebApp.Servicos
{
    public class CasoService
    {
        private readonly PontuaCasosContext _context;

        public CasoService(PontuaCasosContext context)
        {
            _context = context;
        }

        // Ajuste aqui se quiser outras .Includes específicos
        public IQueryable<Caso> IncluirDadosDoCaso()
        {
            return _context.Casos
                .Include(c => c.ItensFamiliares)!.ThenInclude(i => i.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.Item)!.ThenInclude(i => i!.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.ViolenciasSofridas)!.ThenInclude(v => v.Violencia)!.ThenInclude(v => v.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.ViolenciasSofridas)!.ThenInclude(v => v.Situacao)!.ThenInclude(s => s!.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.SituacoesDeSaude)!.ThenInclude(ss => ss.Categoria);
        }

        public IQueryable<ListaCasosViewModel> BaseListaCasos()
        {
            return IncluirDadosDoCaso()
                .Select(c => new ListaCasosViewModel
                {
                    Id = c.Id,
                    Pontos = c.Pontos,
                    ResponsavelFamiliar = c.ResponsavelFamiliar,
                    Titulo = c.Titulo,
                    Prontuario = c.Prontuario,
                    Ativo = c.Ativo,
                    EmAtualizacao = c.EmAtualizacao,
                    CriadoEm = c.CriadoEm,
                    ModificadoEm = c.ModificadoEm,
                    CriadoPor = c.CriadoPor != null && c.CriadoPor.Email != null ? c.CriadoPor.Email : "",
                    ModificadoPor = c.ModificadoPor != null && c.ModificadoPor.Email != null ? c.ModificadoPor.Email : "",
                    CriadoPorId = c.CriadoPorId
                });
        }

        public async Task<ListaPaginavel<ListaCasosViewModel>> ListarCasosPorFiltroAsync(string? filtro, string idUsuario, string? busca, int pagina, int tamanhoPagina)
        {
            if (_context.Casos == null)
                throw new InvalidOperationException("Conjunto de entidades 'PontuaCasosContext.Casos' está nulo.");

            string? administrador = await (from ur in _context.UserRoles
                                           join r in _context.Roles on ur.RoleId equals r.Id
                                           where ur.UserId == idUsuario && (r.Name == "Administradores" || r.Name == "Gestores")
                                           select ur.UserId).FirstOrDefaultAsync();

            var consulta = BaseListaCasos();

            if (administrador == null)
                consulta = consulta.Where(c => c.CriadoPorId == idUsuario);

            if (filtro == null || filtro == "ativos")
                consulta = consulta.Where(c => c.Ativo);
            else if (filtro == "inativos")
                consulta = consulta.Where(c => !c.Ativo);

            if (!String.IsNullOrEmpty(busca))
                consulta = consulta.Where(c => c.Titulo.Contains(busca) || c.ResponsavelFamiliar.Contains(busca) || (c.Prontuario != null && c.Prontuario.Contains(busca)));

            var itens = await consulta
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            var total = await consulta.CountAsync();

            return new ListaPaginavel<ListaCasosViewModel>()
            {
                Itens = itens,
                TotalItens = total,
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina
            };
        }

        public async Task<List<DateTime>> ObterHistoricoDeCasoAsync(int idCaso)
        {
            var lista = new List<DateTime>();

            if (_context.Casos == null)
                throw new InvalidOperationException("Conjunto de entidades 'PontuaCasosContext.Casos' está nulo.");

            string sqlQuery = @"
            WITH CaseChanges AS (
                SELECT 
                    ValidoDe
                FROM Casos
                WHERE Id = @CasoId
                UNION
                SELECT 
                    ValidoDe
                FROM CasosHistorico
                WHERE Id = @CasoId
            ),
            RelatedChanges AS (
                SELECT 
                    v.ValidoDe
                FROM ViolenciasSofridas v
                INNER JOIN IndividuosEmViolacoes iv
                ON v.IndividuoEmViolacaoId = iv.Id
                WHERE iv.CasoId = @CasoId
                UNION
                SELECT 
                    v.ValidoDe
                FROM ViolenciasSofridasHistorico v
                INNER JOIN IndividuosEmViolacoes iv
                ON v.IndividuoEmViolacaoId = iv.Id
                WHERE CasoId = @CasoId
                UNION
                SELECT 
                    ValidoDe
                FROM ItensFamiliares
                WHERE CasoId = @CasoId
                UNION
                SELECT 
                    ValidoDe
                FROM ItensFamiliaresHistorico
                WHERE CasoId = @CasoId
            )
            SELECT * 
            FROM CaseChanges
            UNION 
            SELECT * 
            FROM RelatedChanges
            ORDER BY ValidoDe;
            ";

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    command.Parameters.Add(new SqlParameter("@CasoId", idCaso));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var changeStart = Convert.ToDateTime(reader["ValidoDe"]);
                            lista.Add(changeStart);
                        }
                    }
                }
            }

            return lista;
        }

        public async Task<Caso?> ObterDetalhesDeCasoAsync(int? idCaso)
        {
            if (!idCaso.HasValue)
                return null;

            var caso = await IncluirDadosDoCaso()
                .FirstOrDefaultAsync(m => m.Id == idCaso.Value);

            if (caso != null)
                AgruparCategorias(caso);

            return caso;
        }

        public async Task<Caso?> ObterDetalhesTemporaisDeCasoAsync(int? idCaso, DateTime dataModificacao)
        {
            if (!idCaso.HasValue)
                return null;

            // Carrega caso da tabela temporal
            var casoTemporal = await _context.Casos.TemporalAsOf(dataModificacao)
                .Include(c => c.ItensFamiliares)!.ThenInclude(i => i.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.Item)!.ThenInclude(i => i!.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.ViolenciasSofridas)!.ThenInclude(v => v.Violencia)!.ThenInclude(v => v.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.ViolenciasSofridas)!.ThenInclude(v => v.Situacao)!.ThenInclude(s => s!.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.SituacoesDeSaude)!.ThenInclude(ss => ss.Categoria)
                .FirstAsync();

            if (casoTemporal != null)
                AgruparCategorias(casoTemporal);

            return casoTemporal;
        }

        public async Task<Caso> CriarCasoAsync(Caso caso)
        {
            var resultado = await _context.Casos.AddAsync(caso);
            await _context.SaveChangesAsync();
            return resultado.Entity;
        }

        public async Task AtualizarCasoAsync(Caso caso)
        {
            _context.Casos.Update(caso);
            await _context.SaveChangesAsync();
        }

        public async Task<Caso?> LocalizarCasoAsync(int? idCaso)
        {
            if (!idCaso.HasValue)
                return null;

            return await _context.Casos.FindAsync(idCaso.Value);
        }

        #region Métodos Auxiliares

        private void AgruparCategorias(Caso caso)
        {
            var categorias = new Dictionary<int, Item>();

            if (caso.ItensFamiliares != null)
            {
                foreach (var item in caso.ItensFamiliares)
                {
                    if (item.CategoriaId != null)
                    {
                        if (!categorias.TryGetValue(item.CategoriaId.Value, out var categoriaExistente))
                        {
                            var novaCategoria = _context.Itens
                                .First(i => i.Id == item.CategoriaId.Value);

                            novaCategoria.Itens = new List<Item> { item };
                            categorias.Add(item.CategoriaId.Value, novaCategoria);
                        }
                        else
                        {
                            categoriaExistente.Itens ??= new List<Item>();
                            categoriaExistente.Itens.Add(item);
                        }
                    }
                }
            }
            caso.Categorias = categorias.Values.ToList();
        }

        internal byte[] ExportarAtivos()
        {
            // Example CSV content
            var csvData = new StringBuilder();

            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.CommandText = "ExportarCasos";

                    using (var reader = command.ExecuteReader())
                    {
                        DataTable? schemaTable = reader.GetSchemaTable();

                        if (schemaTable != null)
                        {
                            var linha = new StringBuilder();
                            foreach (DataRow row in schemaTable.Rows)
                                linha.AppendFormat("{0};", row["ColumnName"]);

                            csvData.AppendLine(linha.ToString());
                        }

                        while (reader.Read())
                        {
                            var linha = new StringBuilder();

                            for (int i = 0; i < reader.FieldCount; i++)
                                linha.AppendFormat("{0};", reader[i]);

                            csvData.AppendLine(linha.ToString());
                        }
                    }
                }
            }

            // Convert to byte array
            var bytes = Encoding.UTF8.GetBytes(csvData.ToString());

            // Return the file
            return bytes;
        }

        #endregion
    }
}
