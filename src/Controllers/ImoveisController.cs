using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using click_imoveis.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace click_imoveis.Controllers
{
    [Authorize(Roles = "Corretor, Imobiliária, Administrador, Usuário")]
    public class ImoveisController : Controller
    {
        private readonly AppDbContext _context;

        public ImoveisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Imoveis
        public async Task<IActionResult> Index()
        {
            //bool usuarioLogadoRole = User.IsInRole("Corretor");
            //var usuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? userId = null;

            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
            }

            List<Imovel>? imoveis = new List<Imovel>();


            if (User.IsInRole("Corretor") || User.IsInRole("Imobiliária") || User.IsInRole("Usuário"))
            {
                imoveis = await _context.Imoveis.Where(u => u.UsuarioId == userId).ToListAsync();
            }
            else if (User.IsInRole("Administrador"))
            {
                imoveis = await _context.Imoveis.ToListAsync();
            }

                return View(imoveis);
        }

        // GET: Imoveis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.Midias)
                .FirstOrDefaultAsync(m => m.ImovelId == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // GET: Imoveis/Create
        public IActionResult Create()
        {            
            return View();
        }

        // POST: Imoveis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Imovel imovel, List<IFormFile>? imagens)
        {
            if (imagens != null && imagens.Count > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // Garante que a pasta existe

                var midias = new List<Midia>();

                foreach (var imagem in imagens)
                {
                    if (imagem.Length > 0)
                    {
                        var filePath = Path.Combine(uploadsFolder, imagem.FileName);

                        // Salva a imagem no disco
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagem.CopyToAsync(stream);
                        }

                        // Adiciona o caminho da imagem à lista de mídias
                        midias.Add(new Midia
                        {
                            Link = Path.Combine("uploads", imagem.FileName).Replace("\\", "/"),
                            Imovel = imovel,
                            TipoDeMidia = TipoDeMidia.Imagem
                        });
                    }
                }

                // Associa as mídias ao imóvel
                imovel.Midias = midias;
            }


            if (ModelState.IsValid)
            {   
                // Busca quem é o usuário logado e adiciona à variável imóvel
                var usuarioLogadoId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var usuario = await _context.Usuarios.FindAsync(Int32.Parse(usuarioLogadoId));
                imovel.Usuario = usuario;


                _context.Add(imovel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imovel);
        }

        // GET: Imoveis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel == null)
            {
                return NotFound();
            }
            return View(imovel);
        }

        // POST: Imoveis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Imovel imovel)
        {
            if (id != imovel.ImovelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid && imovel.UsuarioId != null)
            {
                try
                {
                    _context.Update(imovel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImovelExists(imovel.ImovelId))
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
            return View(imovel);
        }

        // GET: Imoveis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .FirstOrDefaultAsync(m => m.ImovelId == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // POST: Imoveis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel != null)
            {
                _context.Imoveis.Remove(imovel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.ImovelId == id);
        }
    }
}
