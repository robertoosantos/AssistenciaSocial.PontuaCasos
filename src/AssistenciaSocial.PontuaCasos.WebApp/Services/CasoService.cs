using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Servicos
{
    public class CasoService
    {
        private readonly PontuaCasosContext _contexto;

        public CasoService(PontuaCasosContext contexto)
        {
            _contexto = contexto;
        }

        // Ajuste aqui se quiser outras .Includes específicos
        public IQueryable<Caso> IncluirDadosDoCaso()
        {
            return _contexto.Casos
                .Include(c => c.ItensFamiliares)!.ThenInclude(i => i.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.Item)!.ThenInclude(i => i!.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.ViolenciasSofridas)!.ThenInclude(v => v.Violencia)!.ThenInclude(v => v.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.ViolenciasSofridas)!.ThenInclude(v => v.Situacao)!.ThenInclude(s => s!.Categoria)
                .Include(c => c.Individuos)!.ThenInclude(i => i.SituacoesDeSaude)!.ThenInclude(ss => ss.Categoria); 
        }

        public async Task<List<Caso>> ListarCasosPorFiltroAsync(string? filtro, string idUsuario)
        {
            if (_contexto.Casos == null)
                throw new InvalidOperationException("Conjunto de entidades 'PontuaCasosContext.Casos' está nulo.");

            var consulta = IncluirDadosDoCaso();
            return filtro switch
            {
                "todos" => await consulta.ToListAsync(),
                "inativos" => await consulta
                    .Where(c => !c.Ativo && c.CriadoPorId == idUsuario)
                    .ToListAsync(),
                _ => await consulta
                    .Where(c => c.Ativo && c.CriadoPorId == idUsuario)
                    .ToListAsync()
            };
        }

        public async Task<List<Caso>> ObterHistoricoDeCasoAsync(int idCaso)
        {
            if (_contexto.Casos == null)
                throw new InvalidOperationException("Conjunto de entidades 'PontuaCasosContext.Casos' está nulo.");

            return await _contexto.Casos
                .TemporalAll()
                .Where(c => c.Id == idCaso)
                .OrderByDescending(c => EF.Property<DateTime>(c, "ValidoAte"))
                .ToListAsync();
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

        public async Task<Caso?> ObterDetalhesTemporaisDeCasoAsync(int? idCaso, DateTime? dataModificacao)
        {
            if (!idCaso.HasValue)
                return null;

            // Se data de modificação não for informada, carrega caso normal
            if (dataModificacao == null)
                return await ObterDetalhesDeCasoAsync(idCaso);

            // Carrega caso da tabela temporal
            var casoTemporal = await _contexto.Casos
                .TemporalAll()
                .FirstOrDefaultAsync(m => m.Id == idCaso && m.ModificadoEm >= dataModificacao.Value);

            if (casoTemporal == null)
                return null;

            // Carrega Individuos & ItensFamiliares também a partir de dados temporais
            await CarregarDadosTemporais(casoTemporal, dataModificacao.Value);
            AgruparCategorias(casoTemporal);

            return casoTemporal;
        }

        public async Task<Caso> CriarCasoAsync(Caso caso)
        {
            var resultado = await _contexto.Casos.AddAsync(caso);
            await _contexto.SaveChangesAsync();
            return resultado.Entity;
        }

        public async Task AtualizarCasoAsync(Caso caso)
        {
            _contexto.Casos.Update(caso);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Caso?> LocalizarCasoAsync(int? idCaso)
        {
            if (!idCaso.HasValue)
                return null;

            return await _contexto.Casos.FindAsync(idCaso.Value);
        }

        #region Métodos Auxiliares

        private async Task CarregarDadosTemporais(Caso caso, DateTime dataModificacao)
        {
            // Carrega IndividuosEmViolacoes
            var individuos = await _contexto.IndividuosEmViolacoes
                .TemporalAsOf(dataModificacao)
                .Where(iv => iv.CasoId == caso.Id)
                .ToListAsync();

            // Carrega Itens Familiares
            caso.ItensFamiliares = await _contexto.ItensFamiliares
                .TemporalAsOf(dataModificacao)
                .Join(
                    _contexto.Itens,
                    ifam => ifam.ItemFamiliarId,
                    it => it.Id,
                    (ifam, it) => new { ifam, it }
                )
                .Where(ifa => ifa.ifam.CasoId == caso.Id)
                .Select(ifa => ifa.it)
                .Include(i => i.Categoria)
                .ToListAsync();

            // Para cada individuo, preencher Item, ViolenciasSofridas, SituacoesDeSaude
            foreach (var ind in individuos)
            {
                ind.Item = await _contexto.Itens
                    .Include(i => i.Categoria)
                    .FirstAsync(it => it.Id == ind.ItemId);

                ind.ViolenciasSofridas = await _contexto.ViolenciasSofridas
                    .TemporalAsOf(dataModificacao)
                    .Where(vs => vs.IndividuoEmViolacao.Id == ind.Id)
                    .ToListAsync();

                ind.SituacoesDeSaude = await _contexto.SitaucoesIndivididuo
                    .TemporalAsOf(dataModificacao)
                    .Join(
                        _contexto.Itens,
                        s => s.ItemSaudeId,
                        item => item.Id,
                        (s, item) => new { s, item }
                    )
                    .Where(si => si.s.IndividuoId == ind.Id)
                    .Select(si => si.item)
                    .Include(x => x.Categoria)
                    .ToListAsync();

                // Preenche Violencia e Situacao
                foreach (var vs in ind.ViolenciasSofridas)
                {
                    vs.Violencia = await _contexto.Itens
                        .Include(i => i.Categoria)
                        .FirstOrDefaultAsync(i => i.Id == vs.ViolenciaId) ?? new Item();

                    if (vs.SituacaoId.HasValue && vs.SituacaoId.Value > 0)
                    {
                        vs.Situacao = await _contexto.Itens
                            .Include(i => i.Categoria)
                            .FirstOrDefaultAsync(i => i.Id == vs.SituacaoId.Value);
                    }
                }
            }

            caso.Individuos = individuos;
        }

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
                            var novaCategoria = _contexto.Itens
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

        #endregion
    }
}
