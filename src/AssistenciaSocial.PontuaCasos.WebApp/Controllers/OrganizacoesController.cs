using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace AssistenciaSocial.PontuaCasos.WebApp.Controllers
{
    [Authorize]
    public class OrganizacoesController : Controller
    {
        private readonly PontuaCasosContext _context;

        public OrganizacoesController(PontuaCasosContext context)
        {
            _context = context;
        }


        // GET: Organizacoes
        public async Task<IActionResult> Index()
        {
              return _context.Organizacoes != null ? 
                          View(await _context.Organizacoes.ToListAsync()) :
                          Problem("Entity set 'PontuaCasosContext.Organizacoes'  is null.");
        }

        // GET: Organizacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Organizacoes == null)
            {
                return NotFound();
            }

            var organizacao = await _context.Organizacoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizacao == null)
            {
                return NotFound();
            }

            return View(organizacao);
        }

        // GET: Organizacoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CriadoEm,ModificadoEm")] Organizacao organizacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organizacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organizacao);
        }

        // GET: Organizacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Organizacoes == null)
            {
                return NotFound();
            }

            var organizacao = await _context.Organizacoes.FindAsync(id);
            if (organizacao == null)
            {
                return NotFound();
            }
            return View(organizacao);
        }

        // POST: Organizacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CriadoEm,ModificadoEm")] Organizacao organizacao)
        {
            if (id != organizacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organizacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizacaoExists(organizacao.Id))
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
            return View(organizacao);
        }

        // GET: Organizacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Organizacoes == null)
            {
                return NotFound();
            }

            var organizacao = await _context.Organizacoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizacao == null)
            {
                return NotFound();
            }

            return View(organizacao);
        }

        // POST: Organizacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Organizacoes == null)
            {
                return Problem("Entity set 'PontuaCasosContext.Organizacoes'  is null.");
            }
            var organizacao = await _context.Organizacoes.FindAsync(id);
            if (organizacao != null)
            {
                _context.Organizacoes.Remove(organizacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizacaoExists(int id)
        {
          return (_context.Organizacoes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
