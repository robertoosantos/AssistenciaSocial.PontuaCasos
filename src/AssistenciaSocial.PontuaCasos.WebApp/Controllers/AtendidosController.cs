using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class AtendidosController : Controller
    {
        private readonly PontuaCasosContext _context;

        public AtendidosController(PontuaCasosContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pontuaCasosContext = _context.Itens.Where(i => !i.Categoria).Include(i => i.CriadoPor).Include(i => i.ModificadoPor);
            return View(await pontuaCasosContext.ToListAsync());
        }

        // GET: Itens/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Itens/Create
        public IActionResult Create()
        {
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            var itens = _context.Itens.Include(i => i.Itens).Where(i => i.Ativo && i.Categoria && i.Multiplo).ToList();
            return View(itens);
        }

        // POST: Itens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? categoria_id, [Bind("Id,Titulo,Pontos,Multiplo")] Item item)
        {
            var user = _context.Users.Include(u => u.Organizacoes).First(u => u.Email == User.Identity.Name);

            if (user.Organizacoes is not null)
                item.Organizacao = user.Organizacoes.First();

            item.Ativo = true;
            item.CriadoEm = DateTime.Now;
            item.ModificadoEm = DateTime.Now;
            item.Categoria = false;

            if (categoria_id is not null)
                item.ItemId = int.Parse(categoria_id);

            item.CriadoPorId = user.Id;
            item.ModificadoPorId = user.Id;

            ModelState.Clear();
            if (!TryValidateModel(item, nameof(item)) || item.ItemId is null)
            {
                return View(item);
            }

            _context.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Categorias", new { id = categoria_id });
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Pontos,Ativo,Multiplo,Categoria,CriadoEm,CriadoPorId,ModificadoPorId,ModificadoEm,OrganizacaoId,ItemId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            var user = _context.Users.First(u => u.Email == User.Identity.Name);
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
                return RedirectToAction("Details", "Categorias", new { id = item.ItemId });
            }
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id", item.CriadoPorId);
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id", item.ModificadoPorId);
            return View(item);
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
