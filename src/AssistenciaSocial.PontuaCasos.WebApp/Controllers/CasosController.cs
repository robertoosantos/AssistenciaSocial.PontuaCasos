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

        public IIncludableQueryable<Caso, Item?> GerarIncludes(DbSet<Caso> caso)
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
                                .ThenInclude(ss => ss.Categoria);
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
                                    .AsSplitQuery()
                                    .ToListAsync()) :
                            Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
                case "inativos":
                    return _context.Casos != null ?
                            View(await GerarIncludes(_context.Casos)
                                    .Where(c => c.Ativo == false && c.CriadoPorId == user.Id)
                                    .AsSplitQuery()
                                    .ToListAsync()) :
                            Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
                default:
                    return _context.Casos != null ?
                            View(await GerarIncludes(_context.Casos)
                                    .Where(c => c.Ativo == true && c.CriadoPorId == user.Id)
                                    .AsSplitQuery()
                                    .ToListAsync()) :
                            Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
            }

        }

        // GET: Casos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //TODO: Tá tudo errado
            if (id == null || _context.Casos == null)
            {
                return NotFound();
            }

            var caso = await _context.Casos
                .Include(c => c.ItensFamiliares!)
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
                .ThenInclude(v => v.Situacao!)
                .ThenInclude(s => s.Categoria)
                .Include(c => c.Individuos!)
                .ThenInclude(i => i.SituacoesDeSaude!)
                .ThenInclude(ss => ss.Categoria)
                .AsSplitQuery()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (caso == null)
            {
                return NotFound();
            }

            var categorias = new Dictionary<int, Item>();

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

            caso.Categorias = categorias.Values.ToList();

            return View(caso);
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

            if (user.Organizacoes != null)
                caso.Organizacao = user.Organizacoes.First();

            ModelState.Clear();
            if (!TryValidateModel(caso, nameof(caso)))
            {
                return View(caso);
            }

            var novoCaso = _context.Add(caso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = novoCaso.Entity.Id });
        }

        // GET: Casos/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(caso);
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
