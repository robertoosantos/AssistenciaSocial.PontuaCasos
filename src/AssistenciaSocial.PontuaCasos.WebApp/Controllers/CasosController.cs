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
    public class CasosController : Controller
    {
        private readonly PontuaCasosContext _context;

        public CasosController(PontuaCasosContext context)
        {
            _context = context;
        }

        // GET: Casos
        public async Task<IActionResult> Index()
        {
            return _context.Casos != null ?
                        View(await _context.Casos.ToListAsync()) :
                        Problem("Entity set 'PontuaCasosContext.Casos'  is null.");
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
                .Include(c => c.Itens)
                .ThenInclude(i => i.Categoria)
                .Include(c => c.Individuos)
                .ThenInclude(i => i.Violencias)
                .ThenInclude(v => v.Situacao)
                .Include(c => c.Individuos)
                .ThenInclude(i => i.Saude)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (caso == null)
            {
                return NotFound();
            }

            var categorias = new Dictionary<int, Item>(); 

            foreach (var item in caso.Itens)
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

            caso.Itens = categorias.Values.ToList();

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
                    if (caso.Itens == null)
                        caso.Itens = new List<Item>();

                    caso.Itens.Add(itemSelecionado);
                }
            }

            caso.CriadoEm = DateTime.Now;
            caso.ModificadoEm = caso.CriadoEm;
            caso.CriadoPorId = user.Id;
            caso.ModificadoPorId = user.Id;
            caso.Ativo = true;
            caso.CalcularPontos();

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Prontuario,ResponsavelFamiliar,Pontos,Ativo,CriadoEm,ModificadoEm")] Caso caso)
        {
            if (id != caso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            return View(caso);
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
