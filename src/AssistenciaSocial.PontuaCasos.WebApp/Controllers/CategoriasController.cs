using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly PontuaCasosContext _context;

        public CategoriasController(PontuaCasosContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return _context.Itens != null ?
                        View(await _context.Itens.ToListAsync()) :
                        Problem("Entity set 'PontuaCasosContext.Itens'  is null.");
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            var item = await _context.Itens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Pontos")] Item item)
        {

            var user = _context.Users.First(u => u.Email == User.Identity.Name);

            item.Ativo = true;
            item.CriadoEm = DateTime.Now;
            item.ModificadoEm = DateTime.Now;
            item.Categoria = true;
            item.CriadoPorId = user.Id;
            item.ModificadoPorId = user.Id;
            item.ModificadoPor = user;
            item.CriadoPor = user;
            item.Organizacao = user.Organizacoes.FirstOrDefault();

            ModelState.Clear();
            if (!TryValidateModel(item, nameof(item)))
            {
                return View(item);
            }

            _context.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Edit/5
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
            return View(item);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Pontos,Ativo,Multiplo,CriadoEm,ModificadoEm")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            var item = await _context.Itens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Categorias/Delete/5
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
                _context.Itens.Remove(item);
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
