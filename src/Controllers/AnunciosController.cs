using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using click_imoveis.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace click_imoveis.Controllers
{
    [Authorize(Roles = "Corretor, Imobiliária, Administrador, Usuário")]
    public class AnunciosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AnunciosController> _logger;

        public AnunciosController(AppDbContext context, ILogger<AnunciosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Anuncios
        public async Task<IActionResult> Index()
        {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? userId = null;

            if (userClaim != null)
            {
                userId = int.Parse(userClaim.Value);
            }

            List<Anuncio> anuncios;

            if (User.IsInRole("Corretor") || User.IsInRole("Imobiliária") || User.IsInRole("Usuário"))
            {
                anuncios = await _context.Anuncios
                    .Include(a => a.Imovel)
                    .Where(u => u.UsuarioId == userId)
                    .ToListAsync();
            }
            else // Administrador
            {
                anuncios = await _context.Anuncios
                    .Include(a => a.Imovel)
                    .ToListAsync();
            }

            return View(anuncios);
        }

        // GET: Anuncios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var anuncio = await _context.Anuncios
                .Include(a => a.Imovel)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.AnuncioId == id);

            if (anuncio == null)
                return NotFound();

            return View(anuncio);
        }

        // GET: Anuncios/Create
        public IActionResult Create()
        {
            ViewData["ImovelId"] = new SelectList(
                _context.Imoveis.Select(i => new
                {
                    i.ImovelId,
                    Descricao = i.Logradouro + ", " + i.Numero + " - " + i.Cidade
                }),
                "ImovelId",
                "Descricao"
            );
            return View();
        }

        // POST: Anuncios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnuncioId,DataInicio,DataFim,Valor,ValorCondominio,ValorIptu,Titulo,Descricao,Finalidade,ImovelId,UsuarioId")] Anuncio anuncio)
        {
            var usuarioLogadoId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (ModelState.IsValid && usuarioLogadoId != null)
            {
                var usuario = await _context.Usuarios.FindAsync(Int32.Parse(usuarioLogadoId));
                anuncio.Usuario = usuario;

                _context.Add(anuncio);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Anúncio criado com sucesso: {AnuncioId}", anuncio.AnuncioId);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ImovelId"] = new SelectList(
                _context.Imoveis.Select(i => new
                {
                    i.ImovelId,
                    Descricao = i.Logradouro + ", " + i.Numero + " - " + i.Cidade
                }),
                "ImovelId",
                "Descricao",
                anuncio.ImovelId
            );

            _logger.LogWarning("Falha ao criar anúncio: {ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return View(anuncio);
        }

        // GET: Anuncios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var anuncio = await _context.Anuncios
                .Include(u => u.Usuario)
                .Include(u => u.Imovel)
                .FirstOrDefaultAsync(u => u.AnuncioId == id);

            if (anuncio == null)
                return NotFound();

            ViewData["ImovelId"] = new SelectList(
                _context.Imoveis.Select(i => new
                {
                    i.ImovelId,
                    Descricao = i.Logradouro + ", " + i.Numero + " - " + i.Cidade
                }),
                "ImovelId",
                "Descricao",
                anuncio.ImovelId
            );

            return View(anuncio);
        }

        // POST: Anuncios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Anuncio anuncio)
        {
            if (id != anuncio.AnuncioId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anuncio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _logger.LogError("Erro de concorrência ao editar o anúncio: {AnuncioId}", anuncio.AnuncioId);
                    if (!AnuncioExists(anuncio.AnuncioId))
                        return NotFound();
                    else
                        throw;
                }
                _logger.LogInformation("Anúncio atualizado com sucesso: {AnuncioId}", anuncio.AnuncioId);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ImovelId"] = new SelectList(
                _context.Imoveis.Select(i => new
                {
                    i.ImovelId,
                    Descricao = i.Logradouro + ", " + i.Numero + " - " + i.Cidade
                }),
                "ImovelId",
                "Descricao",
                anuncio.ImovelId
            );

            _logger.LogWarning("Falha ao editar anúncio: {ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return View(anuncio);
        }

        // GET: Anuncios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var anuncio = await _context.Anuncios
                .Include(a => a.Imovel)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.AnuncioId == id);

            if (anuncio == null)
                return NotFound();

            return View(anuncio);
        }

        // POST: Anuncios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anuncio = await _context.Anuncios.FindAsync(id);
            if (anuncio != null)
            {
                _context.Anuncios.Remove(anuncio);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Anúncio excluído com sucesso: {AnuncioId}", id);
            return RedirectToAction(nameof(Index));
        }

        private bool AnuncioExists(int id)
        {
            return _context.Anuncios.Any(e => e.AnuncioId == id);
        }
    }
}
