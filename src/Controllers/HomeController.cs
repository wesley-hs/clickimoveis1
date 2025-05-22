using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using click_imoveis.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;

namespace click_imoveis.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(string? titulo)
    {

        List<Anuncio> dados = new List<Anuncio>();
        
        if (!string.IsNullOrEmpty(titulo)) 
        {
            dados = await _context.Anuncios
                .Include(a => a.Imovel)
                .ThenInclude(a => a.Midias)
                .Where(c => c.Titulo != null && c.Titulo.Contains(titulo))
                .ToListAsync();
        }
        else
        {
            dados = await _context.Anuncios
                .Include(a => a.Imovel)
                .ThenInclude(a => a.Midias)
                .ToListAsync();
        }

        ViewBag.Titulo = titulo != null ? titulo : null;
           
        return View(dados);
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

    
    public async Task<IActionResult> DetalhesAnuncio(int? id)
    {
        if (id == null)
            return NotFound();

        var anuncio = await _context.Anuncios
                            .Include(u => u.Usuario)
                            .ThenInclude(u => u.PessoaFisica)
                            .Include(u => u.Usuario)
                            .ThenInclude(u => u.PessoaJuridica)
                            .Include(u => u.Imovel)
                            .ThenInclude(u => u.Midias)
                            .FirstOrDefaultAsync(u => u.AnuncioId == id);
                            
        if (anuncio == null)
            return NotFound();

        List<Mensagem> mensagens = await _context.Mensagens
            .Where(u => u.AnuncioId == anuncio.AnuncioId)
            .OrderBy(u => u.DataCriacao)
            .ToListAsync();
        ViewBag.Mensagens = mensagens;

        DetalheAnuncioViewModel detalheAnuncioViewModel = new DetalheAnuncioViewModel
        {
            anuncio = anuncio,
            mensagem = new Mensagem { Conteudo = string.Empty}
        };

        return View(detalheAnuncioViewModel);
    }

    public async Task<IActionResult> CreateMessage(Mensagem mensagem, int anuncioId)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated && ModelState.IsValid)
        {
            mensagem.DataCriacao = DateTime.Now;
            mensagem.AnuncioId = anuncioId;

            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            mensagem.UsuarioId = int.Parse(usuarioIdClaim.Value);

            _context.Mensagens.Add(mensagem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("DetalhesAnuncio", new { id = anuncioId });
    }
}
