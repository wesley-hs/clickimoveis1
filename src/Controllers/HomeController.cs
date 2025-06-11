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

   public IActionResult Index(string titulo, string cidade, int? quartos, float? valorMax, string tipo)
{
    var anuncios = _context.Anuncios
        .Include(a => a.Imovel)
        .ThenInclude(i => i.Midias)
        .AsQueryable();
        if (!string.IsNullOrEmpty(titulo))
            anuncios = anuncios.Where(a => a.Titulo != null && a.Titulo.ToLower().Contains(titulo.ToLower()));

        if (!string.IsNullOrEmpty(cidade))
            anuncios = anuncios.Where(a => a.Imovel.Cidade != null && a.Imovel.Cidade.ToLower().Contains(cidade.ToLower()));

        if (!string.IsNullOrEmpty(titulo))
        anuncios = anuncios.Where(a => a.Titulo.Contains(titulo));

    if (!string.IsNullOrEmpty(cidade))
        anuncios = anuncios.Where(a => a.Imovel.Cidade.Contains(cidade));

    if (quartos.HasValue)
        anuncios = anuncios.Where(a => a.Imovel.Quartos == quartos);

        if (valorMax.HasValue)
            anuncios = anuncios.Where(a => a.Valor <= valorMax);



        // Preenche os ViewBag para manter os filtros na tela
        ViewBag.Titulo = titulo;
    ViewBag.Cidade = cidade;
    ViewBag.Quartos = quartos;
    ViewBag.ValorMax = valorMax;
    ViewBag.Tipo = tipo;

    return View(anuncios.ToList());
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
                            .Include(u => u.Imovel)
                            .ThenInclude(u => u.Comentarios)
                            .ThenInclude(c => c.Usuario)
                            .ThenInclude(c => c.PessoaFisica)
                            .Include(u => u.Imovel)
                            .ThenInclude(u => u.Comentarios)
                            .ThenInclude(c => c.Usuario)
                            .ThenInclude(c => c.PessoaJuridica)
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

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            _logger.LogInformation("Mensagem enviada por {UserEmail} para o anúncio {AnuncioId}.", userEmail, anuncioId);
        }

        return RedirectToAction("DetalhesAnuncio", new { id = anuncioId });
    }

    public async Task<IActionResult> CriarComentario(Comentario comentario, int anuncioId, int imovelId)
    {
        _logger.LogInformation("Criar novo comentário");
        try 
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                comentario.DataCriacao = DateTime.Now;
                comentario.ImovelId = imovelId;

                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                comentario.UsuarioId = int.Parse(usuarioIdClaim.Value);

                await _context.Comentarios.AddAsync(comentario);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Novo anúncio salvo com sucesso.");
            }

        } catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar comentário.");
        }
        
        return RedirectToAction("DetalhesAnuncio", new { id = anuncioId });
        
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var anuncio = await _context.Anuncios.FindAsync(id);
        if (anuncio != null)
        {
            _context.Anuncios.Remove(anuncio);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

}
