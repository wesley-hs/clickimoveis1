using click_imoveis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace click_imoveis.Controllers
{
    public class RelatoriosController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

       
        public IActionResult Index()
        {
            var totalDeImoveisCadastrados = _context.Imoveis.Count();
            ViewBag.totalDeImoveisCadastrados = totalDeImoveisCadastrados;

            var totalDeAnunciosCadastrados = _context.Anuncios.Count();
            ViewBag.totalDeAnunciosCadastrados = totalDeAnunciosCadastrados;

            var totalDeUsuariosCadastrados = _context.Usuarios.Count();
            ViewBag.totalDeUsuariosCadastrados = totalDeUsuariosCadastrados;

            return View();
        }
    }
}
