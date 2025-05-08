using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using click_imoveis.Models;
using Microsoft.EntityFrameworkCore;

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
                .Where(c => c.Titulo != null && c.Titulo.Contains(titulo))
                .ToListAsync();
        }
        else
        {
            dados = await _context.Anuncios.ToListAsync();
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

    
    public async Task<IActionResult> DetalhesAnuncio(int id)
    {
        if (id == null)
            return NotFound();

        var anuncio = await _context.Anuncios.FindAsync(id);
        if (anuncio == null)
            return NotFound();

        return View(anuncio);
    }
}
