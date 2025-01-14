using Microsoft.AspNetCore.Mvc;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using AssistenciaSocial.PontuaCasos.WebApp.Servicos;
using Microsoft.EntityFrameworkCore;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    public class CasosController : Controller
    {
        private readonly CasoService _servicoDeCasos;
        private readonly PontuaCasosContext _context;

        public CasosController(PontuaCasosContext contexto)
        {
            _context = contexto;
            _servicoDeCasos = new CasoService(_context);
        }

        /// <summary>
        /// Exemplo de como carregar o usuário atual, assumindo que User.Identity.Name == id do usuário (string).
        /// Ajuste conforme sua forma real de autenticação/identificação de usuário.
        /// </summary>
        private Usuario UsuarioAtual =>
            _context.Users
                .Include(u => u.Organizacoes)
                .First(u => User.Identity != null && u.Email == User.Identity!.Name);

        // GET: Casos
        public async Task<IActionResult> Index(string? filtro, string? busca, int pagina = 1, int tamanhoPagina = 10)
        {
            try
            {
                var usuario = UsuarioAtual; // userId é string
                var idUsuario = usuario.Id;  // string
                var lista = await _servicoDeCasos.ListarCasosPorFiltroAsync(filtro, idUsuario, busca, pagina, tamanhoPagina);
                ViewData["busca"] = busca;
                ViewData["filtro"] = filtro;
                return View(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET: Casos/History
        public async Task<IActionResult> History(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var historico = await _servicoDeCasos.ObterHistoricoDeCasoAsync(id.Value);
            return View(historico);
        }

        // GET: Casos/Details/5
        public async Task<IActionResult> Details(int? id, string? modificado_em)
        {
            if (!id.HasValue)
                return NotFound();

            DateTime modificadoEm;

            Caso? caso = null;

            if (DateTime.TryParse(modificado_em, out modificadoEm))
                caso = await _servicoDeCasos.ObterDetalhesTemporaisDeCasoAsync(id, modificadoEm);
            else
                caso = await _servicoDeCasos.ObterDetalhesDeCasoAsync(id);

            if (caso == null)
                return NotFound();

            return View(caso);
        }

        // GET: Casos/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = ConsultarCategorias(null);
            return View();
        }

        // POST: Casos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Titulo,Prontuario,ResponsavelFamiliar,CriadoPorId,ModificadoPorId")]
            Caso caso)
        {
            var usuario = UsuarioAtual;

            PreencherItensFamiliares(caso);

            // Preenche campos básicos
            caso.CriadoEm = DateTime.Now;
            caso.ModificadoEm = caso.CriadoEm;
            caso.CriadoPorId = usuario.Id;       // string
            caso.ModificadoPorId = usuario.Id;   // string
            caso.Ativo = true;
            caso.EmAtualizacao = true;

            if (usuario.Organizacoes?.Any() == true)
            {
                caso.Organizacao = usuario.Organizacoes.First();
            }

            ModelState.ClearValidationState(nameof(caso));
            if (!TryValidateModel(caso, nameof(caso)))
            {
                ViewBag.Categorias = ConsultarCategorias(null);
                return View(caso);
            }

            var novoCaso = await _servicoDeCasos.CriarCasoAsync(caso);
            return RedirectToAction(nameof(Edit), new { id = novoCaso.Id });
        }

        // GET: Casos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var caso = await _servicoDeCasos.ObterDetalhesDeCasoAsync(id);
            if (caso == null)
                return NotFound();

            if (!caso.EmAtualizacao)
            {
                caso.EmAtualizacao = true;
                await _servicoDeCasos.AtualizarCasoAsync(caso);
            }

            ViewBag.Categorias = ConsultarCategorias(caso);
            ViewData["Editando"] = true;

            return View(caso);
        }

        // GET: Casos/EditConfirmed/5
        public async Task<IActionResult> EditConfirmed(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var caso = await _servicoDeCasos.LocalizarCasoAsync(id.Value);
            if (caso == null)
                return NotFound();

            var usuario = UsuarioAtual;

            caso.EmAtualizacao = false;
            caso.ModificadoEm = DateTime.Now;
            caso.ModificadoPorId = usuario.Id; // string

            await _servicoDeCasos.AtualizarCasoAsync(caso);
            return RedirectToAction(nameof(Index));
        }

        // POST: Casos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Titulo,Prontuario,ResponsavelFamiliar,Pontos,Ativo,CriadoEm,CriadoPorId,ModificadoPorId")]
            Caso caso)
        {
            if (id != caso.Id)
                return NotFound();

            var usuario = UsuarioAtual;

            var casoBanco = await _context.Casos
                .Include(c => c.ItensFamiliares)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (casoBanco == null)
                return View(caso);

            // Recarrega itens familiares
            casoBanco.ItensFamiliares?.Clear();
            casoBanco.ItensFamiliares ??= new List<Item>();

            PreencherItensFamiliares(casoBanco);

            // Atualiza campos
            casoBanco.Titulo = caso.Titulo;
            casoBanco.ResponsavelFamiliar = caso.ResponsavelFamiliar;
            casoBanco.Prontuario = caso.Prontuario;
            casoBanco.ModificadoEm = DateTime.Now;
            casoBanco.ModificadoPorId = usuario.Id; // string

            ModelState.Clear();
            if (!TryValidateModel(casoBanco, nameof(caso)))
            {
                return View(caso);
            }

            try
            {
                _context.Update(casoBanco);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Casos.Any(e => e.Id == casoBanco.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Casos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var caso = await _servicoDeCasos.LocalizarCasoAsync(id.Value);
            if (caso == null)
                return NotFound();

            return View(caso);
        }

        // POST: Casos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caso = await _servicoDeCasos.LocalizarCasoAsync(id);
            if (caso == null)
                return Problem("Caso não encontrado.");

            caso.Ativo = false;
            await _servicoDeCasos.AtualizarCasoAsync(caso);

            return RedirectToAction(nameof(Index));
        }

        #region Métodos Auxiliares

        /// <summary>
        /// Lê dados do form para popular ItensFamiliares no Caso.
        /// Ajuste de acordo com a maneira como o seu form POSTa os dados.
        /// </summary>
        private void PreencherItensFamiliares(Caso caso)
        {
            // Exemplo: a chave no form pode ser algo como "itens_123".
            foreach (var par in Request.Form.Where(f => f.Value.Any(v => v.Contains("itens_"))))
            {
                foreach (var valor in par.Value)
                {
                    if (valor.StartsWith("itens_"))
                    {
                        if (int.TryParse(valor.Replace("itens_", ""), out var idItem))
                        {
                            var itemSelecionado = _context.Itens
                                .Include(i => i.Categoria)
                                .FirstOrDefault(i => i.Id == idItem);

                            if (itemSelecionado != null)
                            {
                                caso.ItensFamiliares ??= new List<Item>();
                                caso.ItensFamiliares.Add(itemSelecionado);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Exemplo de consulta às categorias para exibir na tela de criação/edição.
        /// </summary>
        private List<ViewModelCategoriaFamiliar> ConsultarCategorias(Caso? caso)
        {
            var lista = new List<ViewModelCategoriaFamiliar>();

            var categorias = _context.Itens
                .IncludeSubItensAtivos()
                .Where(i => i.Ativo && i.ECategoria && i.UnicaPorFamilia)
                .ToList();

            foreach (var cat in categorias)
            {
                var vmCat = new ViewModelCategoriaFamiliar
                {
                    Categoria = cat,
                    Itens = new List<ViewModelItemFamiliar>()
                };

                if (cat.Itens == null) 
                    continue;

                foreach (var subItem in cat.Itens)
                {
                    var selecionado = caso?.ItensFamiliares?.Exists(i => i.Id == subItem.Id) ?? false;
                    vmCat.Itens.Add(new ViewModelItemFamiliar
                    {
                        Item = subItem,
                        Selecionado = selecionado
                    });
                }

                lista.Add(vmCat);
            }
            return lista;
        }

        #endregion
    }
}