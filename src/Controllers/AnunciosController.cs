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
        public async Task<IActionResult> Index(string searchString)
        {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? userId = null;

            if (userClaim != null)
            {
                userId = int.Parse(userClaim.Value);
            }

            IQueryable<Anuncio> anunciosQuery;

            if (User.IsInRole("Corretor") || User.IsInRole("Imobiliária") || User.IsInRole("Usuário"))
            {
                anunciosQuery = _context.Anuncios
                    .Include(a => a.Imovel)
                    .Include(a => a.Usuario)
                    .Where(u => u.UsuarioId == userId);
            }
            else // Administrador
            {
                anunciosQuery = _context.Anuncios
                    .Include(a => a.Imovel)
                    .Include(a => a.Usuario);
            }


            if (!string.IsNullOrEmpty(searchString))
            {
                var search = searchString.ToLower();
                anunciosQuery = anunciosQuery.Where(a =>
                    (a.Titulo != null && a.Titulo.ToLower().Contains(search)) ||
                    (a.Descricao != null && a.Descricao.ToLower().Contains(search)) ||
                    (a.Usuario != null && a.Usuario.Email.ToLower().Contains(search))
                );
            }


            ViewData["CurrentFilter"] = searchString; // <-- Adicione esta linha
            var anuncios = await anunciosQuery.ToListAsync();
            _logger.LogInformation("Total de anúncios encontrados: {Count}", anuncios.Count);
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
        // GET: Anuncios/Pesquisar
        [AllowAnonymous] // Se quiser permitir acesso público
        public async Task<IActionResult> Pesquisar(string searchString)
        {
            var anunciosQuery = _context.Anuncios
                .Include(a => a.Imovel)
                .Include(a => a.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = searchString.ToLower();
                anunciosQuery = anunciosQuery.Where(a =>
                    (a.Titulo != null && a.Titulo.ToLower().Contains(search)) ||
                    (a.Descricao != null && a.Descricao.ToLower().Contains(search)) ||
                    (a.Usuario != null && a.Usuario.Email.ToLower().Contains(search))
                );
            }

            ViewData["CurrentFilter"] = searchString;
            var anuncios = await anunciosQuery.ToListAsync();
            return View(anuncios);
        }

    }
}
