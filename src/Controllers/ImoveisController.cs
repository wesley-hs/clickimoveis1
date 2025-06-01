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
        private readonly ILogger<ImoveisController> _logger;

        public ImoveisController(AppDbContext context, ILogger<ImoveisController> logger)
        {
            _context = context;
            _logger = logger;
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
                _logger.LogWarning("Imóvel fornecido com id nulo para detalhes.");
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.Midias)
                .FirstOrDefaultAsync(m => m.ImovelId == id);
            if (imovel == null)
            {
                _logger.LogWarning("Imóvel com ID {id} não encontrado.", id);
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
            List<string> caminhoDasImagens = new List<string>();
            List<Midia> midias = new List<Midia>();

            if (imagens != null && imagens.Count > 0)
            {
                caminhoDasImagens = await SalvarImagens(imagens);

                foreach (var caminho in caminhoDasImagens)
                {
                    midias.Add(new Midia
                    {
                        Link = caminho,
                        Imovel = imovel,
                        TipoDeMidia = TipoDeMidia.Imagem
                    });
                }

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

                _logger.LogInformation("Imóvel criado com sucesso: {imovelId}", imovel.ImovelId);
                return RedirectToAction(nameof(Index));
            }
            return View(imovel);
        }

        // GET: Imoveis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Imóvel fornecido com id nulo para edição.");
                return NotFound();
            }

            var imovel = await _context.Imoveis
                                    .Include(u => u.Midias)
                                    .FirstOrDefaultAsync(u => u.ImovelId == id);
            if (imovel == null)
            {
                _logger.LogWarning("Imóvel com ID {id} não encontrado para edição.", id);
                return NotFound();
            }
            return View(imovel);
        }

        // POST: Imoveis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Imovel imovel, string imagensRemovidas, List<IFormFile>? imagens)
        {
            if (id != imovel.ImovelId)
            {
                _logger.LogWarning("Tentativa de edição de imóvel com ID {id} diferente do ID fornecido {imovelId}.", id, imovel.ImovelId);
                return NotFound();
            }

            if (imovel.Midias == null)
            {
                imovel.Midias = new List<Midia>();
            }

            List<Midia> midiasExistentes = await _context.Midias.Where(u => u.ImovelId == imovel.ImovelId).ToListAsync();
                      
            if (midiasExistentes.Count > 0)
            {               

                foreach (var midia in midiasExistentes)
                {
                    imovel.Midias.Add(midia);
                }
            }
           

            List<string> caminhosNovasMidias = new List<string>();
            
            if (imagens != null && imagens.Count > 0)
            {
                caminhosNovasMidias = await SalvarImagens(imagens);
                
                foreach (var caminho in caminhosNovasMidias)
                {
                    imovel.Midias.Add(new Midia
                    {
                        Link = caminho,
                        Imovel = imovel,
                        TipoDeMidia = TipoDeMidia.Imagem
                    });

                }

            }


            ModelState.Remove("imagensRemovidas");



                if (ModelState.IsValid && imovel.UsuarioId != null)
                {
                
                    _context.Update(imovel);                    
                    await _context.SaveChangesAsync();

                    if (imagensRemovidas != null)
                    {
                    var imagensParaRemover = imagensRemovidas
                       .Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(id => int.Parse(id))
                       .ToList();

                    var midiasToRemove = await _context.Midias
                            .Where(m => imagensParaRemover.Contains(m.MidiaId))
                            .ToListAsync();

                    _context.Midias.RemoveRange(midiasToRemove);
                    await _context.SaveChangesAsync();
                    }


                _logger.LogInformation("Imóvel editado com sucesso: {imovelId}", imovel.ImovelId);
                return RedirectToAction(nameof(Index));
                }

            _logger.LogWarning("Erro ao editar imóvel com ID {id}. Verifique os dados fornecidos.", id);
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

            _logger.LogInformation("Imóvel com ID {id} deletado com sucesso.", id);
            return RedirectToAction(nameof(Index));
        }

        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.ImovelId == id);
        }

        public async Task<List<string>> SalvarImagens(List<IFormFile> imagens)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder); // Garante que a pasta existe

            List<string> caminhoDasImagens = new List<string>();

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

                    caminhoDasImagens.Add(Path.Combine("uploads", imagem.FileName).Replace("\\", "/"));
                    
                }
            }

            return caminhoDasImagens;
        }
    }
}
