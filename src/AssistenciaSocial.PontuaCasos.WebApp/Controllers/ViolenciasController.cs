using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class ViolenciasController : Controller
    {
        private readonly PontuaCasosContext _context;
        private readonly ItemService _item;

        public ViolenciasController(PontuaCasosContext context)
        {
            _context = context;
            _item = new ItemService(context);
        }

        // GET: Itens/Create
        public IActionResult Create()
        {
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            List<Item> itens = _item.ConsultarViolencias();

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

            if (individuo.Caso == null)
            {
                ViewData["Erro"] = "Não é possível criar um indivíduo sem um caso associado.";
                return View(_item.ConsultarViolencias());  
            }

            var user = _context.Users
                .Include(u => u.Organizacoes)
                .First(u => User.Identity != null && u.Email == User.Identity!.Name);

            var idViolencia = int.Parse(Request.Form[Item.ITENS_VIOLENCIAS][0]);
            var idSituacao = int.Parse(Request.Form[Item.ITENS_SITUACAO_VIOLENCIAS][0]);

            if (idViolencia == 0)
            {
                ViewData["Erro"] = "Selecione uma violência.";
                return View(_item.ConsultarViolencias());   
            }

            var violencia = _context.Itens.Include(i => i.Categoria).First(i => i.Id == idViolencia);
            var situacao = _context.Itens.Include(i => i.Categoria).FirstOrDefault(i => i.Id == idSituacao);

            if (individuo.ViolenciasSofridas == null)
                individuo.ViolenciasSofridas = new List<ViolenciaSofrida>();

            individuo.ViolenciasSofridas.Add(new ViolenciaSofrida
            {
                Violencia = violencia,
                Situacao = situacao,
                IndividuoEmViolacao = individuo
            });

            individuo.Caso.ModificadoEm = DateTime.Now;
            individuo.Caso.ModificadoPorId = user.Id;

            ModelState.Clear();
            if (!TryValidateModel(individuo, nameof(individuo)))
            {
                return View(_item.ConsultarViolencias());
            }

            _context.Update(individuo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ViewData["Erro"] = "Este ciclo já possui registro dessa violência.";
                return View(_item.ConsultarViolencias());
            }

            return RedirectToAction(nameof(IndividuosController.Edit), "Individuos", new { id = individuo.Id });
        }

        // GET: Itens/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            var item = await _context.ViolenciasSofridas
                .Include(vs => vs.IndividuoEmViolacao)
                .Include(vs => vs.Violencia)
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Itens == null)
            {
                return Problem("Entity set 'PontuaCasosContext.Itens'  is null.");
            }
            var item = await _context.ViolenciasSofridas.Include(vs => vs.IndividuoEmViolacao).SingleOrDefaultAsync(vs => vs.Id == id);

            if (item != null)
                _context.ViolenciasSofridas.Remove(item);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndividuosController.Edit), "Individuos", new { id = item.IndividuoEmViolacao.Id });
        }

        private bool ItemExists(int id)
        {
            return (_context.Itens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
