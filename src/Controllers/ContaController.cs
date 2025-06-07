using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using click_imoveis.Models;

namespace click_imoveis.Controllers
{
    public class ContaController : Controller
    {
        private readonly AppDbContext _context;

        public ContaController(AppDbContext context)
        {
            _context = context;
        }

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

        // GET: Conta/Editar
        public async Task<IActionResult> Editar()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Index");

            int userId = int.Parse(userIdClaim.Value);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return RedirectToAction("Index");

            var model = new ContaViewModel { Usuario = usuario };
            return View(model);
        }

        // POST: Conta/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ContaViewModel model)
        {
            if (ModelState.IsValid && model.Usuario != null)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    TempData["MensagemErro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index");
                }

                int userId = int.Parse(userIdClaim.Value);
                var usuario = await _context.Usuarios.FindAsync(userId);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index");
                }
                // Processa o upload da foto
                if (model.FotoUpload != null && model.FotoUpload.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "perfil");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.FotoUpload.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FotoUpload.CopyToAsync(stream);
                    }

                    usuario.FotoUrl = $"/uploads/perfil/{fileName}";
                }

                // Atualize os campos permitidos
                usuario.Email = model.Usuario.Email;
                usuario.Creci = model.Usuario.Creci;
                usuario.Logradouro = model.Usuario.Logradouro;
                usuario.Numero = model.Usuario.Numero;
                usuario.Complemento = model.Usuario.Complemento;
                usuario.Bairro = model.Usuario.Bairro;
                usuario.Cidade = model.Usuario.Cidade;
                usuario.Estado = model.Usuario.Estado;
                usuario.CodigoPostal = model.Usuario.CodigoPostal;
                usuario.TelefonePrincipal = model.Usuario.TelefonePrincipal;
                usuario.TelefoneAlternativo1 = model.Usuario.TelefoneAlternativo1;
                usuario.TelefoneAlternativo2 = model.Usuario.TelefoneAlternativo2;
                usuario.FotoUrl = model.Usuario.FotoUrl;

                _context.Update(usuario);
                await _context.SaveChangesAsync();

                TempData["MensagemSucesso"] = "Dados atualizados com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Erro ao atualizar dados.";
            return View(model);
        }

        // GET: Conta/AlterarSenha
        public IActionResult AlterarSenha()
        {
            return View();
        }

        // POST: Conta/AlterarSenha

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    TempData["MensagemErro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index");
                }

                int userId = int.Parse(userIdClaim.Value);
                var usuario = await _context.Usuarios.FindAsync(userId);
                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index");
                }

                // Use BCrypt para validar a senha atual
                if (!BCrypt.Net.BCrypt.Verify(model.SenhaAtual, usuario.Senha))
                {
                    TempData["MensagemErro"] = "Senha atual incorreta.";
                    return View(model);
                }

                if (model.NovaSenha != model.ConfirmarNovaSenha)
                {
                    TempData["MensagemErro"] = "A nova senha e a confirmação não coincidem.";
                    return View(model);
                }

                // Salve a nova senha já criptografada
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(model.NovaSenha);
                _context.Update(usuario);
                await _context.SaveChangesAsync();

                TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Erro ao alterar senha.";
            return View(model);
        }



        // POST: Conta/Excluir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            int userId = int.Parse(userIdClaim.Value);
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Conta excluída com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
            }

            // Redirecione para a página inicial ou de login
            return RedirectToAction("Index", "Home");
        }
    }
}
