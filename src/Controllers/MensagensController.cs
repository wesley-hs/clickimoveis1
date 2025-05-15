using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using click_imoveis.Models;

namespace click_imoveis.Controllers
{
    public class MensagensController : Controller
    {
        private readonly AppDbContext _context;

        public MensagensController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Mensagens
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Mensagens.Include(m => m.Anuncio).Include(m => m.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Mensagens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagens
                .Include(m => m.Anuncio)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.MensagemId == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        // GET: Mensagens/Create
        public IActionResult Create()
        {
            ViewData["AnuncioId"] = new SelectList(_context.Anuncios, "AnuncioId", "AnuncioId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email");
            return View();
        }

        // POST: Mensagens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MensagemId,DataCriacao,Conteudo,UsuarioId,AnuncioId")] Mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mensagem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnuncioId"] = new SelectList(_context.Anuncios, "AnuncioId", "AnuncioId", mensagem.AnuncioId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email", mensagem.UsuarioId);
            return View(mensagem);
        }

        // GET: Mensagens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagens.FindAsync(id);
            if (mensagem == null)
            {
                return NotFound();
            }
            ViewData["AnuncioId"] = new SelectList(_context.Anuncios, "AnuncioId", "AnuncioId", mensagem.AnuncioId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email", mensagem.UsuarioId);
            return View(mensagem);
        }

        // POST: Mensagens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MensagemId,DataCriacao,Conteudo,UsuarioId,AnuncioId")] Mensagem mensagem)
        {
            if (id != mensagem.MensagemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mensagem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MensagemExists(mensagem.MensagemId))
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
            ViewData["AnuncioId"] = new SelectList(_context.Anuncios, "AnuncioId", "AnuncioId", mensagem.AnuncioId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Email", mensagem.UsuarioId);
            return View(mensagem);
        }

        // GET: Mensagens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensagem = await _context.Mensagens
                .Include(m => m.Anuncio)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.MensagemId == id);
            if (mensagem == null)
            {
                return NotFound();
            }

            return View(mensagem);
        }

        // POST: Mensagens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mensagem = await _context.Mensagens.FindAsync(id);
            if (mensagem != null)
            {
                _context.Mensagens.Remove(mensagem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MensagemExists(int id)
        {
            return _context.Mensagens.Any(e => e.MensagemId == id);
        }
    }
}
