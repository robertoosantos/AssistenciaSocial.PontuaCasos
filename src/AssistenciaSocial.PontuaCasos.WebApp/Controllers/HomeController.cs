using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PontuaCasosContext _context;

    public HomeController(ILogger<HomeController> logger, PontuaCasosContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var home = new Models.Home();

        if (User!.Identity!.IsAuthenticated)
        {
            var user = _context.Users.Include(u => u.Organizacoes).First(u => u.Email == User.Identity.Name);

            home.CasosEmAtualizacao = CasosController.GerarIncludes(_context.Casos).Where(c => c.EmAtualizacao == true && (c.CriadoPorId == user.Id || c.ModificadoPorId == user.Id)).ToList();

            home.CasosSemAtualizacao = CasosController.GerarIncludes(_context.Casos).Where(c => (c.CriadoPorId == user.Id || c.ModificadoPorId == user.Id) && EF.Functions.DateDiffMonth(c.ModificadoEm, DateTime.Now) >= 6).ToList();
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
