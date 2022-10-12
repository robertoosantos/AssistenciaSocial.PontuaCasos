using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class CasosController : Controller
    {
        private readonly PontuaCasosContext _context;

        public CasosController(PontuaCasosContext context)
        {
            _context = context;
        }

        public static IQueryable<Caso> GerarIncludes(DbSet<Caso> caso)
        {
            return caso.Include(c => c.ItensFamiliares!)
                                .ThenInclude(i => i.Categoria)
                                .Include(c => c.Individuos!)
                                .ThenInclude(i => i.Item!)
                                .ThenInclude(i => i.Categoria)
                                .Include(c => c.Individuos!)
                                .ThenInclude(i => i.ViolenciasSofridas!)
                                .ThenInclude(v => v.Violencia!)
                                .ThenInclude(v => v.Categoria)
                                .Include(c => c.Individuos!)
                                .ThenInclude(i => i.ViolenciasSofridas!)
                                .ThenInclude(v => v.Situacao)
                                .ThenInclude(s => s.Categoria)
                                .Include(c => c.Individuos!)
                                .ThenInclude(i => i.SituacoesDeSaude!)
                                .ThenInclude(ss => ss.Categoria)
                                .AsSplitQuery();
        }

        // GET: Casos
        public async Task<IActionResult> Index(string? filtro)
        {
            var user = _context.Users.Include(u => u.Organizacoes).First(u => User.Identity != null && u.Email == User.Identity.Name);

            switch (filtro)
            {
                case "todos":
                    return _context.Casos != null ?
                            View(await GerarIncludes(_context.Casos)
                                    .ToListAsync()) :
                            Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
                case "inativos":
                    return _context.Casos != null ?
                            View(await GerarIncludes(_context.Casos)
                                    .Where(c => c.Ativo == false && c.CriadoPorId == user.Id)
                                    .ToListAsync()) :
                            Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
                default:
                    return _context.Casos != null ?
                            View(await GerarIncludes(_context.Casos)
                                    .Where(c => c.Ativo == true && c.CriadoPorId == user.Id)
                                    .ToListAsync()) :
                            Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
            }

        }

        // GET: Casos/History
        public async Task<IActionResult> History(int? id)
        {
            return _context.Casos != null ?
                    View(await _context.Casos
                            .TemporalAll()
                            .Where(c => c.Id == id)
                            .OrderByDescending(c => EF.Property<DateTime>(c, "ValidoAte"))
                            .ToListAsync()) :
                    Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
        }

        // GET: Casos/Details/5
        public async Task<IActionResult> Details(int? id, DateTime? modificado_em)
        {
            if (id == null || _context.Casos == null)
            {
                return NotFound();
            }

            var caso = await ConsultarItem(id, modificado_em);

            if (caso == null)
            {
                return NotFound();
            }

            return View(caso);
        }

        private async Task<Caso?> ConsultarItem(int? id, DateTime? modificadoEm)
        {
            if (modificadoEm == null)
                return await ConsultarItem(id);

            var caso = await _context.Casos
            .TemporalAll()
            .FirstOrDefaultAsync(m => m.Id == id && m.ModificadoEm >= modificadoEm.Value);

            if (caso == null)
            {
                return null;
            }

            var individuos = await _context.IndividuosEmViolacoes
                                .TemporalAsOf(modificadoEm.Value)
                                .Where(iv => iv.CasoId == caso.Id).ToListAsync();

            caso.ItensFamiliares = await _context.ItensFamiliares
                                    .TemporalAsOf(modificadoEm.Value)
                                    .Join(_context.Itens,
                                    i => i.ItemFamiliarId,
                                    it => it.Id,
                                    (i, it) => new { i, it })
                                    .Where(ifa => ifa.i.CasoId == caso.Id)
                                    .Select(ifa => ifa.it)
                                    .Include(i => i.Categoria)
                                    .ToListAsync();

            foreach (var i in individuos)
            {
                i.Item = await _context.Itens.Include(i => i.Categoria).FirstAsync(it => it.Id == i.ItemId);

                i.ViolenciasSofridas = await _context.ViolenciasSofridas
                                        .TemporalAsOf(modificadoEm.Value)
                                        .Where(vs => vs.IndividuoEmViolacao.Id == i.Id)
                                        .ToListAsync();

                i.SituacoesDeSaude = await _context.SitaucoesIndivididuo
                                        .TemporalAsOf(modificadoEm.Value)
                                        .Join(_context.Itens,
                                        s => s.ItemSaudeId,
                                        i => i.Id,
                                        (s, i) => new { i, s })
                                        .Where(si => si.s.IndividuoId == i.Id)
                                        .Select(x => x.i)
                                        .Include(x => x.Categoria)
                                        .ToListAsync();

                foreach (var v in i.ViolenciasSofridas)
                {
                    v.Violencia = await _context.Itens
                                    .Include(i => i.Categoria)
                                    .FirstOrDefaultAsync(i => i.Id == v.ViolenciaId);

                    if (v.SituacaoId != null && v.SituacaoId > 0)
                        v.Situacao = await _context.Itens
                                        .Include(i => i.Categoria)
                                        .FirstOrDefaultAsync(i => i.Id == v.SituacaoId);
                }
            }

            caso.Individuos = individuos;
            AgruparCategorias(caso);

            return caso;
        }

        private void AgruparCategorias(Caso? caso)
        {
            var categorias = new Dictionary<int, Item>();

            if (caso.ItensFamiliares != null)
            {
                foreach (var item in caso.ItensFamiliares)
                {
                    if (item.CategoriaId != null)
                    {
                        Item? existe;

                        var categoria = _context.Itens.First(i => i.Id == item.CategoriaId);

                        if (categorias.TryGetValue((int)item.CategoriaId, out existe))
                        {
                            if (existe.Itens != null)
                                existe.Itens.Add(item);
                        }
                        else
                        {

                            categoria.Itens = new List<Item>();
                            categoria.Itens.Add(item);
                            categorias.Add((int)item.CategoriaId, categoria);
                        }
                    }
                }
            }

            caso.Categorias = categorias.Values.ToList();
        }

        private async Task<Caso?> ConsultarItem(int? id)
        {
            var caso = await GerarIncludes(_context.Casos)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (caso == null)
            {
                return null;
            }


            AgruparCategorias(caso);

            return caso;
        }

        // GET: Casos/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = _context.Itens.Include(i => i.Itens).Where(i => i.Ativo && i.ECategoria && i.UnicaPorFamilia).ToList();
            return View();
        }

        // POST: Casos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Prontuario,ResponsavelFamiliar")] Caso caso)
        {
            var user = _context.Users.Include(u => u.Organizacoes).First(u => User.Identity != null && u.Email == User.Identity.Name);

            foreach (var item in Request.Form.Where(f => f.Value[0].Contains("itens")))
            {
                var id = int.Parse(item.Value[0].Replace("itens_", ""));
                var itemSelecionado = _context.Itens.Include(i => i.Categoria).FirstOrDefault(i => i.Id == id);
                if (itemSelecionado != null && itemSelecionado.CategoriaId != null)
                {
                    if (caso.ItensFamiliares == null)
                        caso.ItensFamiliares = new List<Item>();

                    caso.ItensFamiliares.Add(itemSelecionado);
                }
            }

            caso.CriadoEm = DateTime.Now;
            caso.ModificadoEm = caso.CriadoEm;
            caso.CriadoPorId = user.Id;
            caso.ModificadoPorId = user.Id;
            caso.Ativo = true;
            caso.EmAtualizacao = true;

            if (user.Organizacoes != null)
                caso.Organizacao = user.Organizacoes.First();

            ModelState.Clear();
            if (!TryValidateModel(caso, nameof(caso)))
            {
                return View(caso);
            }

            var novoCaso = _context.Add(caso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = novoCaso.Entity.Id });
        }

        // GET: Casos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Casos == null)
            {
                return NotFound();
            }

            var caso = await ConsultarItem(id);
            if (caso == null)
            {
                return NotFound();
            }

            if (!caso.EmAtualizacao)
            {
                caso.EmAtualizacao = true;
                _context.Update(caso);
                await _context.SaveChangesAsync();
            }

            ViewData["Editando"] = true;

            return View(caso);
        }

        // GET: Casos/Edit/5
        public async Task<IActionResult> EditConfirmed(int? id)
        {
            if (id == null || _context.Casos == null)
            {
                return NotFound();
            }

            var caso = await _context.Casos.FindAsync(id);
            if (caso == null)
            {
                return NotFound();
            }

            var user = _context.Users.Include(u => u.Organizacoes).First(u => User.Identity != null && u.Email == User.Identity.Name);

            caso.EmAtualizacao = false;
            caso.ModificadoEm = DateTime.Now;
            caso.ModificadoPorId = user.Id;

            _context.Update(caso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Casos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Prontuario,ResponsavelFamiliar,Pontos,Ativo,CriadoEm,CriadoPorId,ModificadoPorId")] Caso caso)
        {
            if (id != caso.Id)
            {
                return NotFound();
            }

            var user = _context.Users.Include(u => u.Organizacoes).First(u => User.Identity != null && u.Email == User.Identity.Name);
            caso.ModificadoEm = DateTime.Now;
            caso.ModificadoPorId = user.Id;

            ModelState.Clear();
            if (!TryValidateModel(caso, nameof(caso)))
            {
                return View(caso);
            }

            try
            {
                _context.Update(caso);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CasoExists(caso.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Casos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Casos == null)
            {
                return NotFound();
            }

            var caso = await _context.Casos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caso == null)
            {
                return NotFound();
            }

            return View(caso);
        }

        // POST: Casos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Casos == null)
            {
                return Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
            }
            var caso = await _context.Casos.FindAsync(id);
            if (caso != null)
            {
                _context.Casos.Remove(caso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasoExists(int id)
        {
            return (_context.Casos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
