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
            var itens = _context.Itens.Include(i => i.Itens).Where(i => i.Ativo && i.Categoria && !i.UnicaPorFamilia).ToList();
            foreach (var item in itens)
            {
                if (item.Itens != null)
                    item.Itens.Insert(0, new Item { Id = 0, Titulo = "" });
            }
            return View(itens);
        }

        // POST: Itens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? caso_id, [Bind("Id,Titulo,Pontos")] Item item)
        {
            var categorias = new List<Item>();
            var itens = new List<ItensCasos>();
            Item? itemUnico = null;

            var caso = _context.Casos.Include(c => c.Itens).First(c => caso_id != null && c.Id == int.Parse(caso_id));
            var user = _context.Users.Include(u => u.Organizacoes).First(u => User.Identity != null && u.Email == User.Identity.Name);

            foreach (string idItem in Request.Form["Itens"])
            {
                var id = int.Parse(idItem);

                if (id > 0)
                {
                    var itemSelecionado = _context.Itens.FirstOrDefault(i => i.Id == id);

                    if (itemSelecionado != null && itemSelecionado.ItemId != null)
                    {
                        var categoria = _context.Itens.First(i => i.Id == itemSelecionado.ItemId);
                        categorias.Add(categoria);

                        if (categoria.UnicaPorAtendido)
                        {
                            itemUnico = itemSelecionado;
                        }

                        itens.Add(new ItensCasos()
                        {
                            ItemId = itemSelecionado.Id,
                            CasoId = caso.Id
                        });
                    }
                }
            }

            if (itemUnico != null)
            {
                foreach (var itemCaso in itens)
                {
                    if (itemCaso.ItemId != itemUnico.Id)
                        itemCaso.ItemPai = itemUnico.Id;
                }
            }

            ModelState.Clear();
            if (!TryValidateModel(caso, nameof(caso)))
            {
                return View(caso);
            }

            _context.AddRange(itens);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Casos", new { id = caso_id });
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
