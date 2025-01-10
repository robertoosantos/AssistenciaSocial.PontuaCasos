using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Servicos;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PontuaCasosContext _context;
    private readonly CasoService _servicoDeCasos;

    public HomeController(ILogger<HomeController> logger, PontuaCasosContext context)
    {
        _logger = logger;
        _context = context;
        _servicoDeCasos = new CasoService(_context);
    }

    public async Task<IActionResult> Index()
    {
        var home = new Models.Home();

        if (User!.Identity!.IsAuthenticated)
        {
            var user = await _context.Users.Include(u => u.Organizacoes).FirstAsync(u => u.Email == User.Identity.Name);

            home.CasosEmAtualizacao = await _servicoDeCasos.IncluirDadosDoCaso().Where(c => c.Ativo && c.EmAtualizacao == true && (c.CriadoPorId == user.Id || c.ModificadoPorId == user.Id)).ToListAsync();

            home.CasosSemAtualizacao = await _servicoDeCasos.IncluirDadosDoCaso().Where(c => c.Ativo && (c.CriadoPorId == user.Id || c.ModificadoPorId == user.Id) && EF.Functions.DateDiffMonth(c.ModificadoEm, DateTime.Now) >= 6).ToListAsync();
        }

        return View(home);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
