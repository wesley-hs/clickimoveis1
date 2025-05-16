using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using click_imoveis.Models;
using System.Security.Claims;

namespace click_imoveis.Controllers
{
    public class ContaController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;
               

        // GET: Conta
        public async Task<IActionResult> Index()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim.Value);
                Usuario? usuario = await _context.Usuarios
                                        .Include(u => u.PessoaFisica)
                                        .Include(u => u.PessoaJuridica)
                                        .FirstOrDefaultAsync(u => u.UsuarioId == userId);

                ICollection<Imovel>? imoveis = await _context.Imoveis
                                        .Where(u => u.UsuarioId == userId)
                                        .ToListAsync();

                ICollection<Anuncio>? anuncios = await _context.Anuncios
                                        .Where(u => u.UsuarioId == userId)
                                        .ToListAsync();

                ContaViewModel contaViewModel = new ContaViewModel
                {
                    Usuario = usuario,
                    PessoaFisica = usuario?.PessoaFisica,
                    PessoaJuridica = usuario?.PessoaJuridica,
                    Imoveis = imoveis,
                    Anuncios = anuncios
                };

                return View(contaViewModel);


            }


            return Redirect("/");
        }

     
                      

       

        
    }
}
