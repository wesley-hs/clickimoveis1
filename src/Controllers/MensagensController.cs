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
 
    public class MensagensController : Controller
    {
        private readonly AppDbContext _context;

        public MensagensController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MensagemId,DataCriacao,Conteudo,UsuarioId,AnuncioId")] Mensagem mensagem)
        {
            var usuarioLogadoId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (ModelState.IsValid && usuarioLogadoId != null)
            {
                // Busca quem é o usuário logado e adiciona à variável Mensagem
                var usuario = await _context.Usuarios.FindAsync(Int32.Parse(usuarioLogadoId));
                mensagem.Usuario = usuario;

                _context.Add(mensagem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
 
            return View(mensagem);
        }

        
    }
}
