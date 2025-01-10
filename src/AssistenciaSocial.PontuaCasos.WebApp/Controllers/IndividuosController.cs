using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class IndividuosController : Controller
    {
        private readonly PontuaCasosContext _context;
        private readonly Item _item;

        public IndividuosController(PontuaCasosContext context)
        {
            _context = context;
            _item = new Item(context);
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

        // GET: Itens/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }
            IndividuoEmViolacao? individuo = await ConsultarIndividuos(id);

            if (individuo == null)
            {
                return NotFound();
            }

            return View(individuo);
        }

        private async Task<IndividuoEmViolacao?> ConsultarIndividuos(string? id)
        {
            var individuo = await _context.IndividuosEmViolacoes
                            .Include(i => i.Item)
                            .Include(i => i.Caso)
                            .Include(i => i.ViolenciasSofridas!)
                            .ThenInclude(v => v.Violencia)
                            .Include(i => i.ViolenciasSofridas!)
                            .ThenInclude(v => v.Situacao)
                            .Include(i => i.SituacoesDeSaude)
                            .SingleOrDefaultAsync(i => i.Id == id);

            if (individuo != null)
            {
                individuo.OpcoesViolencias = _item.ConsultarViolencias();
                individuo.OpcoesSaude = SaudeController.ConsultarItens(_context);
            }

            return individuo;
        }

        // POST: Itens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id")] IndividuoEmViolacao individuo)
        {
            if (id != individuo.Id)
            {
                return NotFound();
            }

            var user = _context.Users.First(u => User.Identity != null && u.Email == User.Identity.Name);
            var individuoDb = _context.IndividuosEmViolacoes
                                .Include(i => i.Caso)
                                .Include(i => i.ViolenciasSofridas)
                                .Include(i => i.SituacoesDeSaude)
                                .First(i => i.Id == id);

            individuoDb.Caso!.ModificadoEm = DateTime.Now;
            individuoDb.Caso.ModificadoPorId = user.Id;

            for (int i = 0; i < Request.Form[Item.ITENS_VIOLENCIAS].Count; i++)
            {
                var idViolencia = int.Parse(Request.Form[Item.ITENS_VIOLENCIAS][i]);

                if (idViolencia == 0 || idViolencia == int.MaxValue)
                {
                    ViewData["Erro"] = "Selecione uma violência.";
                    return View(ConsultarIndividuos(id));
                }

                individuoDb.ViolenciasSofridas![i].ViolenciaId = idViolencia;
            }

            for (int i = 0; i < Request.Form[Item.ITENS_SITUACAO_VIOLENCIAS].Count; i++)
            {
                var idSituacao = int.Parse(Request.Form[Item.ITENS_SITUACAO_VIOLENCIAS][i]);

                if (idSituacao != 0 && idSituacao != int.MaxValue)
                {
                    individuoDb.ViolenciasSofridas![i].SituacaoId = idSituacao;
                } else {
                    individuoDb.ViolenciasSofridas![i].SituacaoId = null;
                }
            }
 
            for (int i = 0; i < Request.Form[Item.ITENS_SAUDE].Count; i++)
            {
                var idSaude = int.Parse(Request.Form[Item.ITENS_SAUDE][i]);
                var saude = await _context.Itens.FirstAsync(s => s.Id == idSaude);

                individuoDb.SituacoesDeSaude![i] = saude;
            }

            ModelState.Clear();
            if (!TryValidateModel(individuo, nameof(individuo)))
            {
                return View(ConsultarIndividuos(id));
            }

            try
            {
                _context.Update(individuoDb);
                await _context.SaveChangesAsync();
            }   
            catch (Exception ex)
            {
                if (!IndividuoExists(individuo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(CasosController.Edit), "Casos", new { id = individuoDb.CasoId });
        }

        // GET: Itens/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            var item = await _context.IndividuosEmViolacoes
                .Include(i => i.Item)
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
            int casoId = 0;

            if (_context.Itens == null)
            {
                return Problem("Entity set 'PontuaCasosContext.Itens'  is null.");
            }

            var item = await _context.IndividuosEmViolacoes.FindAsync(id);
            if (item != null)
            {
                casoId = item.CasoId;
                _context.IndividuosEmViolacoes.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CasosController.Edit), "Casos", new { id = casoId });
        }

        private bool IndividuoExists(string id)
        {
            return (_context.IndividuosEmViolacoes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
