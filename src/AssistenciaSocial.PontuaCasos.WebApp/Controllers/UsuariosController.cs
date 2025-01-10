using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    [Authorize(Roles = "Gestores")]
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
                usuario.LockoutEnd = null;

                _context.Update(usuario);

                await _userManager.AddToRoleAsync(usuario, Perfil.PERFIL_USUARIOS);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Casos == null)
            {
                return NotFound();
            }

            var usuario = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            var usuario = await _context.Users.Include(u => u.Organizacoes).SingleOrDefaultAsync(u => u.Id == id);

            if (usuario != null && usuario.Email != User.Identity.Name)
            {
                usuario.Organizacoes = null;
                await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.MaxValue);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}