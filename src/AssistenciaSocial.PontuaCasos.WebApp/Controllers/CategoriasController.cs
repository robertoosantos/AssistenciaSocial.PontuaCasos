using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;

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
            var mensagensAlerta = new StringBuilder();
            var filtroCategoriasObrigatorias = new List<string> { Item.ITENS_ATENDIDOS, Item.ITENS_SAUDE, Item.ITENS_VIOLENCIAS, Item.ITENS_SITUACAO_VIOLENCIAS };
            
            var categoriasUsuario = await _context.Itens.Where(i => i.UnicaPorAtendido == true && i.Ativo == true).ToListAsync();
            var categoriasObrigatorias = await _context.Itens.Where(i => filtroCategoriasObrigatorias.Contains(i.Titulo)).ToListAsync();
            
            if (categoriasUsuario.Count > 1)
                 mensagensAlerta.AppendLine("Há mais de uma categoria ativa que Identifica os Atendidos. Por favor, marque apenas uma categoria com essa opção.");

            if (categoriasUsuario.Count < 1)
                 mensagensAlerta.AppendLine(string.Format("Não há uma categoria ativa que Identifica os Atendidos. Por favor, crie ou atualize a categoria {0} e marque esta opção.", Item.ITENS_ATENDIDOS));

            if (categoriasObrigatorias.Count < filtroCategoriasObrigatorias.Count)
            {
                foreach (var filtro in filtroCategoriasObrigatorias)
                {
                    var categoriaLocalizada = false;

                    foreach (var categoria in categoriasObrigatorias)
                    {
                        if (categoria.Titulo == filtro){
                            categoriaLocalizada = true;
                            break;
                        }
                    }

                    if (!categoriaLocalizada)
                        mensagensAlerta.AppendLine(string.Format("Não foi localizada a categoria obrigatória: {0}. Por favor, crie uma categoria com esse título ou altere um título existente para: {0}", filtro));
                }
            }

            ViewData["Alerta"] = mensagensAlerta.ToString();

            return _context.Itens != null ?
                        View(await _context.Itens.Where(i => i.ECategoria).ToListAsync()) :
                        Problem("Entity set 'PontuaCasosContext.Itens'  is null.");
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Itens == null)
            {
                return NotFound();
            }

            var item = await _context.Itens.Include(i => i.Itens)
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Pontos,UnicaPorFamilia,UnicaPorAtendido")] Item item)
        {
            var user = _context.Users.Include(u => u.Organizacoes).First(u => u.Email == User.Identity!.Name);

            if (user.Organizacoes is not null)
                item.Organizacao = user.Organizacoes.First();

            item.Ativo = true;
            item.CriadoEm = DateTime.Now;
            item.ModificadoEm = DateTime.Now;
            item.ECategoria = true;
            item.CriadoPorId = user.Id;
            item.ModificadoPorId = user.Id;

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Pontos,Ativo,UnicaPorFamilia,UnicaPorAtendido,CriadoEm,CriadoPorId,ModificadoPorId,ModificadoEm,OrganizacaoId,ItemId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            var user = _context.Users.First(u => u.Email == User.Identity!.Name);
            item.ECategoria = true;
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
