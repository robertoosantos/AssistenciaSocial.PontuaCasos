using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class IndividuosController : Controller
    {
        private readonly PontuaCasosContext _context;

        public IndividuosController(PontuaCasosContext context)
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

            return View();
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

            var idViolencia = int.Parse(Request.Form[ViolenciasController.ITENS_VIOLENCIAS][0]);
            var idSituacao = int.Parse(Request.Form[ViolenciasController.ITENS_SITUACAO_VIOLENCIAS][0]);

            if (idViolencia == 0)
            {
                ViewData["Erro"] = "Selecione uma violência.";
                return View();
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
                return View();
            }

            _context.Update(individuo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ViewData["Erro"] = "Esta pessoa já possui registro dessa violência.";
                return View();
            }

            return RedirectToAction(nameof(Details), "Casos", new { id = individuo.Caso.Id });
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
                individuo.OpcoesViolencias = ViolenciasController.ConsultarItens(_context);
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

            for (int i = 0; i < Request.Form[ViolenciasController.ITENS_VIOLENCIAS].Count; i++)
            {
                var idViolencia = int.Parse(Request.Form[ViolenciasController.ITENS_VIOLENCIAS][i]);

                if (idViolencia == 0 || idViolencia == int.MaxValue)
                {
                    ViewData["Erro"] = "Selecione uma violência.";
                    return View(ConsultarIndividuos(id));
                }

                individuoDb.ViolenciasSofridas![i].ViolenciaId = idViolencia;
            }

            for (int i = 0; i < Request.Form[ViolenciasController.ITENS_SITUACAO_VIOLENCIAS].Count; i++)
            {
                var idSituacao = int.Parse(Request.Form[ViolenciasController.ITENS_SITUACAO_VIOLENCIAS][i]);

                if (idSituacao != 0 && idSituacao != int.MaxValue)
                {
                    individuoDb.ViolenciasSofridas![i].SituacaoId = idSituacao;
                } else {
                    individuoDb.ViolenciasSofridas![i].SituacaoId = null;
                }
            }
 
            for (int i = 0; i < Request.Form[SaudeController.ITENS_SAUDE].Count; i++)
            {
                var idSaude = int.Parse(Request.Form[SaudeController.ITENS_SAUDE][i]);
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

            return RedirectToAction(nameof(CasosController.Details), "Casos", new { id = individuoDb.CasoId });
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
            return RedirectToAction(nameof(CasosController.Details), "Casos", new { id = casoId });
        }

        private bool IndividuoExists(string id)
        {
            return (_context.IndividuosEmViolacoes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
