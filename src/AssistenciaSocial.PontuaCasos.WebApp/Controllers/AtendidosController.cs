using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class AtendidosController : Controller
    {
        internal const string ITENS_ATENDIDOS = "Ciclos de Vida";

        private readonly PontuaCasosContext _context;

        public AtendidosController(PontuaCasosContext context)
        {
            _context = context;
        }

        // GET: Itens/Create
        public IActionResult Create()
        {
            ViewData["CriadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ModificadoPorId"] = new SelectList(_context.Users, "Id", "Id");
            List<Item> itens = ConsultarItens();
            return View(itens);
        }

        private List<Item> ConsultarItens()
        {
            var itens = _context.Itens.Include(i => i.Itens).Where(i => i.Ativo && i.ECategoria && i.UnicaPorAtendido).ToList();
            foreach (var item in itens)
            {
                if (item.Itens != null)
                {
                    item.Itens.Insert(0, new Item { Id = 0, Titulo = "", Pontos = int.MaxValue });
                }
            }

            return itens;
        }

        // POST: Itens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? caso_id, [Bind("Id,Titulo,Pontos")] Item item)
        {
            var caso = _context.Casos.Include(c => c.Individuos).First(c => caso_id != null && c.Id == int.Parse(caso_id));
            var user = _context.Users.Include(u => u.Organizacoes).First(u => User.Identity != null && u.Email == User.Identity.Name);
            IndividuoEmViolacao? individuo = null;

            foreach (string idItem in Request.Form[ITENS_ATENDIDOS])
            {
                var id = int.Parse(idItem);

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
                return View(ConsultarItens());
            }

            _context.Update(caso);
            await _context.SaveChangesAsync();

            if (individuo != null)
                return RedirectToAction(nameof(IndividuosController.Edit), "Individuos", new { id = individuo.Id });
            else
                return View(ConsultarItens());
        }
    }
}
