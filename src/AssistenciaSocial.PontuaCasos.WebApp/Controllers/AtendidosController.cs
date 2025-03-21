using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class AtendidosController : Controller
    {
        private readonly PontuaCasosContext _context;
        private readonly ItemService _item;

        public AtendidosController(PontuaCasosContext context)
        {
            _context = context;
            _item = new ItemService(context);
        }

        // GET: Itens/Create
        public IActionResult Create()
        {
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            List<Item> itens = _item.ConsultarAtendidos();
            return View(itens);
        }

        // POST: Itens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? caso_id, [Bind("Id,Titulo,Pontos")] Item item)
        {
            var caso = _context.Casos.Include(c => c.Individuos).First(c => caso_id != null && c.Id == int.Parse(caso_id));
            var user = _context.Users.Include(u => u.Organizacoes).First(u => u.Email == User.Identity!.Name);
            IndividuoEmViolacao? individuo = null;

            if (Request.Form[Item.ITENS_ATENDIDOS].Count == 0)
            {
                ViewBag.ErrorMessage = String.Format("{0} não foi informado.", Item.ITENS_ATENDIDOS);
            }
            else
            {
                foreach (string? idItem in Request.Form[Item.ITENS_ATENDIDOS])
                {
                    var id = 0;
                    
                    int.TryParse(idItem, out id);

                    if (id != int.MaxValue)
                    {
                        var itemSelecionado = _context.Itens.Include(i => i.Categoria).FirstOrDefault(i => i.Id == id);

                        if (itemSelecionado != null && itemSelecionado.Categoria != null)
                        {
                            if (caso.Individuos == null)
                                caso.Individuos = new List<IndividuoEmViolacao>();

                            individuo = new IndividuoEmViolacao
                            {
                                ItemId = itemSelecionado.Id,
                                CasoId = caso.Id
                            };

                            caso.Individuos.Add(individuo);
                        }
                    }
                }

                caso.ModificadoEm = DateTime.Now;
                caso.ModificadoPorId = user.Id;

                ModelState.Clear();
                if (!TryValidateModel(caso, nameof(caso)))
                {
                    return View(_item.ConsultarAtendidos());
                }

                _context.Update(caso);
                await _context.SaveChangesAsync();
            }
            
            if (individuo != null)
                return RedirectToAction(nameof(IndividuosController.Edit), "Individuos", new { id = individuo.Id });
            else
                return View(_item.ConsultarAtendidos());
        }
    }
}
