using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    [Authorize(Roles="Gestores")]
    public class UsuariosController : Controller
    {
        private readonly PontuaCasosContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsuariosController(PontuaCasosContext context, RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.Include(u => u.Organizacoes).ToListAsync()) :
                        Problem("Entity set 'PontuaCasosContext.Users'  is null.");
        }

        public async Task<IActionResult> Aprovar(string? id)
        {
            var usuario = await _context.Users.Include(u => u.Organizacoes).SingleOrDefaultAsync(u => u.Id == id);

            var user = _context.Users
                .Include(u => u.Organizacoes)
                .First(u => User.Identity != null && u.Email == User.Identity.Name);

            if (usuario != null)
            {
                if (usuario.Organizacoes == null)
                    usuario.Organizacoes = new List<Organizacao>();

                usuario.Organizacoes = user.Organizacoes;
                
                _context.Update(usuario);

                await _userManager.AddToRoleAsync(usuario, Perfil.PERFIL_USUARIOS);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}