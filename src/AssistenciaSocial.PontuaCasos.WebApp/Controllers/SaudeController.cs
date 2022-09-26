using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class SaudeController : Controller
    {
        internal const string ITENS_SAUDE = "Condições de Saúde do Violado";
        private readonly PontuaCasosContext _context;

        public SaudeController(PontuaCasosContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pontuaCasosContext = _context.Itens.Where(i => !i.ECategoria).Include(i => i.CriadoPor).Include(i => i.ModificadoPor);
            return View(await pontuaCasosContext.ToListAsync());
        }

        // GET: Itens/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.IndividuosEmViolacoes == null)
            {
                return NotFound();
            }

            var item = await _context.IndividuosEmViolacoes
                .Include(i => i.Item)
                .ThenInclude(i => i.CriadoPor)
                .Include(i => i.Item)
                .ThenInclude(i => i.ModificadoPor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Itens/Create
        public IActionResult Create()
        {
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            List<Item> itens = ConsultarItens();

            return View(itens);
        }

        // POST: Itens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? individuo_id, [Bind("Id,Titulo,Pontos")] Item item)
        {
            var individuo = _context.IndividuosEmViolacoes
                .Include(i => i.Caso)
                .First(i => i.Id == individuo_id);

            var user = _context.Users
                .Include(u => u.Organizacoes)
                .First(u => User.Identity != null && u.Email == User.Identity.Name);

            var idSaude = int.Parse(Request.Form[ITENS_SAUDE][0]);

            var saude = _context.Itens.Include(i => i.Categoria).First(i => i.Id == idSaude);

            if (individuo.SituacoesDeSaude == null)
                individuo.SituacoesDeSaude = new List<Item>();

            individuo.SituacoesDeSaude.Add(saude);

            individuo.Caso.ModificadoEm = DateTime.Now;
            individuo.Caso.ModificadoPorId = user.Id;

            ModelState.Clear();
            if (!TryValidateModel(individuo, nameof(individuo)))
            {
                return View(ConsultarItens());
            }

            _context.Update(individuo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ViewData["Erro"] = "Este indivíduo já possui essa situação de saúde.";
                return View(ConsultarItens());
            }

            return RedirectToAction(nameof(Details), "Casos", new { id = individuo.Caso.Id });
        }

        private List<Item> ConsultarItens()
        {
            return ConsultarItens(_context);
        }

        // GET: Itens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            var item = await _context.Itens.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id", item.CriadoPorId);
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id", item.ModificadoPorId);
            return View(item);
        }

        // POST: Itens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Pontos,Ativo,UnicaPorFamilia,Categoria,CriadoEm,CriadoPorId,ModificadoPorId,ModificadoEm,OrganizacaoId,ItemId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            var user = _context.Users.First(u => User.Identity != null && u.Email == User.Identity.Name);
            item.ModificadoEm = DateTime.Now;
            item.ModificadoPorId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Categorias", new { id = item.CategoriaId });
            }
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id", item.CriadoPorId);
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id", item.ModificadoPorId);
            return View(item);
        }

        internal static List<Item> ConsultarItens(PontuaCasosContext context)
        {
            var itens = context.Itens.Include(i => i.Itens).Where(i => i.Titulo == ITENS_SAUDE).OrderByDescending(i => i.Pontos).ToList();

            foreach (var item in itens)
            {
                if (item.Itens != null)
                {
                    item.Itens.Insert(0, new Item { Id = 0, Titulo = "", Pontos = int.MaxValue });
                }
            }

            return itens;
        }

        // GET: Itens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            var item = await _context.Itens
                .Include(i => i.CriadoPor)
                .Include(i => i.ModificadoPor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Itens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Itens == null)
            {
                return Problem("Entity set 'PontuaCasosContext.Itens'  is null.");
            }
            var item = await _context.Itens.FindAsync(id);
            if (item != null)
            {
                item.Ativo = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return (_context.Itens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
