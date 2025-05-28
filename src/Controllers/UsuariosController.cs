using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using click_imoveis.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace click_imoveis.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(AppDbContext context, ILogger<UsuariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            //var resultado = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? userId = null;

            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
            }

            List<Usuario>? usuarios = new List<Usuario>();

            if (User.IsInRole("Corretor") || User.IsInRole("Imobiliária") || User.IsInRole("Usuário"))
            {            
                usuarios = await _context.Usuarios
                    .Where(u => u.UsuarioId == userId)
                    .Include(u => u.PessoaFisica)
                    .Include(u => u.PessoaJuridica)
                    .ToListAsync();

            } else if (User.IsInRole("Administrador"))
            {
                usuarios = await _context.Usuarios
                    .Include(u => u.PessoaFisica)
                    .Include(u => u.PessoaJuridica)
                    .ToListAsync();
            }



                return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.PessoaFisica)
                .Include(u => u.PessoaJuridica)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);


            //var usuario = await _context.Usuarios
            //    .FirstOrDefaultAsync(m => m.UsuarioId == id);


            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
                      

            ViewBag.Pessoas = GetListaPessoa();
            ViewBag.TipoDeUsuario = GetListaTipoDeUsuario();

            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel usuarioViewModel)
        {            
            ViewBag.Pessoas = GetListaPessoa();
            ViewBag.TipoDeUsuario = GetListaTipoDeUsuario();

            // Remove propriedades não utilizadas do ModelState
            if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaFisica)
            {
                ModelState.Remove("PessoaFisica.Usuario");
                foreach (var key in ModelState.Keys.Where(k => k.StartsWith("PessoaJuridica")))
                {
                    ModelState.Remove(key);
                }

            } else if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaJuridica)
            {
                ModelState.Remove("PessoaJuridica.Usuario");
                foreach (var key in ModelState.Keys.Where(k => k.StartsWith("PessoaFisica")))
                {
                    ModelState.Remove(key);
                }
            }


            if (!ModelState.IsValid)
            {
                return View(usuarioViewModel);
            }
            
            // Hash password
            usuarioViewModel.Usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioViewModel.Usuario.Senha);

            // Insere a data atual no cadastro
            usuarioViewModel.Usuario.DataCadastro = DateTime.Now;

            // Save Usuario first
            _context.Add(usuarioViewModel.Usuario);
            await _context.SaveChangesAsync(); // Now UsuarioId is generated

            // Now depending on Pessoa type
            if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaFisica)
            {
                
                    usuarioViewModel.PessoaFisica.UsuarioId = usuarioViewModel.Usuario.UsuarioId;
                    _context.Add(usuarioViewModel.PessoaFisica);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");
                
            }
            else if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaJuridica)
            {
                
                    usuarioViewModel.PessoaJuridica.UsuarioId = usuarioViewModel.Usuario.UsuarioId;
                    _context.Add(usuarioViewModel.PessoaJuridica);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");
                
            }

            // If something failed (like PessoaFisica or PessoaJuridica validation)
           
            return View(usuarioViewModel);

            
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var usuario = await _context.Usuarios
                .Include(u => u.PessoaFisica)
                .Include(u => u.PessoaJuridica)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Map Usuario to UsuarioViewModel
#pragma warning disable CS8601 // Possible null reference assignment.
            var usuarioViewModel = new UsuarioViewModel
            {
                Usuario = usuario,
                PessoaFisica = usuario.Pessoa == Pessoa.PessoaFisica ? usuario.PessoaFisica : null,
                PessoaJuridica = usuario.Pessoa == Pessoa.PessoaJuridica ? usuario.PessoaJuridica : null
            };
#pragma warning restore CS8601 // Possible null reference assignment.


            return View(usuarioViewModel);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel usuarioViewModel)
        {
            if (id != usuarioViewModel.Usuario.UsuarioId)
            {
                return NotFound();
            }

            // Remove propriedades não utilizadas do ModelState
            if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaFisica)
            {
                ModelState.Remove("PessoaFisica.Usuario");
                foreach (var key in ModelState.Keys.Where(k => k.StartsWith("PessoaJuridica")))
                {
                    ModelState.Remove(key);
                }

            }
            else if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaJuridica)
            {
                ModelState.Remove("PessoaJuridica.Usuario");
                foreach (var key in ModelState.Keys.Where(k => k.StartsWith("PessoaFisica")))
                {
                    ModelState.Remove(key);
                }
            }

            if (ModelState.IsValid)
            {
                Usuario? usuario = await _context.Usuarios.FindAsync(usuarioViewModel.Usuario.UsuarioId);
                if (usuario != null)
                {
                    usuario.Email = usuarioViewModel.Usuario.Email;
                    usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioViewModel.Usuario.Senha);
                    usuario.TipoDeUsuario = usuarioViewModel.Usuario.TipoDeUsuario;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                } else
                {
                    return View(usuarioViewModel);
                }

                if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaFisica)
                {
                    PessoaFisica? pessoaFisica = await _context.PessoasFisicas.FirstOrDefaultAsync(u => u.UsuarioId == usuarioViewModel.Usuario.UsuarioId);

                    if (pessoaFisica != null)
                    {
                        pessoaFisica.NomeCompleto = usuarioViewModel.PessoaFisica.NomeCompleto;
                        _context.Update(pessoaFisica);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        
                        PessoaFisica pessoaFisicaNova = new PessoaFisica
                        {
                            NomeCompleto = usuarioViewModel.PessoaFisica.NomeCompleto, // Ensure NomeCompleto is set
                            Usuario = null, // O null é devido ao EF já estar trackeando o Usuario e evitar trackear 2 vezes
                            UsuarioId = usuarioViewModel.Usuario.UsuarioId
                        };
                        _context.Add(pessoaFisicaNova);
                        await _context.SaveChangesAsync();

                    }

                        
                    
                }

                
                if (usuarioViewModel.Usuario.Pessoa == Pessoa.PessoaJuridica)
                {
                    PessoaJuridica? pessoaJuridica = await _context.PessoasJuridicas.FirstOrDefaultAsync(u => u.UsuarioId == usuarioViewModel.Usuario.UsuarioId);

                    if (pessoaJuridica != null)
                    {
                        pessoaJuridica.RazaoSocial = usuarioViewModel.PessoaJuridica.RazaoSocial;
                        _context.Update(pessoaJuridica);
                        await _context.SaveChangesAsync();

                    } else
                    {
                        PessoaJuridica pessoaJuridicaNova = new PessoaJuridica
                        {
                            RazaoSocial = usuarioViewModel.PessoaJuridica.RazaoSocial,
                            Usuario = null, // O null é devido ao EF já estar trackeando o Usuario e evitar trackear 2 vezes
                            UsuarioId = usuarioViewModel.Usuario.UsuarioId
                        };
                        _context.Add(pessoaJuridicaNova);
                        await _context.SaveChangesAsync();
                    }
                }
               
               
                return RedirectToAction(nameof(Index));
            }

            return View(usuarioViewModel);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var dados = await _context.Usuarios.FirstOrDefaultAsync(c => c.Email == usuario.Email);

            if (dados == null)
            {
                _logger.LogWarning("Tentativa de login de usuário não cadastrado no sistema: {emailUsuario}",usuario.Email);
                
                ViewBag.Message = "Usuário e/ou senha inválidos.";
                return View();
            }

            bool senhaOk = BCrypt.Net.BCrypt.Verify(usuario.Senha, dados.Senha);

            if (senhaOk)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dados.Email),
                    new Claim(ClaimTypes.Email, dados.Email),
                    new Claim(ClaimTypes.NameIdentifier, dados.UsuarioId.ToString()),
                    new Claim(ClaimTypes.Role, dados.TipoDeUsuario.ToString())
                };

                var usuarioIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(usuarioIdentity);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.ToLocalTime().AddHours(8),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(principal, props);

                _logger.LogInformation("Usuário {dados.Email} logado com sucesso.",dados.Email);

                return Redirect("/");
                

            }
            else
            {
                ViewBag.Message = "Usuário e/ou senha inválidos.";
                
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        private List<SelectListItem> GetListaPessoa()
        {
            var listaPessoa = Enum.GetValues(typeof(Pessoa)) //pega todos os valores do enum Pessoa
                    .Cast<Pessoa>()                     // converte para enum fortemente tipado
                    .Select(e => new SelectListItem     // monta cada item do tipo SelectListItem
                    {
                        Value = e.ToString(),
                        Text = e.ToString()
                    }).ToList();

            // Inserir a opção vazia manualmente
            listaPessoa.Insert(0, new SelectListItem { Text = "-- Selecione --", Value = "" });
            return listaPessoa;
        }

        private List<SelectListItem> GetListaTipoDeUsuario()
        {
            var listaTipoDeUsuario = Enum.GetValues(typeof(TipoDeUsuario))
                   .Cast<TipoDeUsuario>()
                   .Select(e => new SelectListItem
                   {
                       Value = e.ToString(),
                       Text = e.ToString()
                   }).ToList();

            // Inserir a opção vazia manualmente
            listaTipoDeUsuario.Insert(0, new SelectListItem { Text = "-- Selecione --", Value = "" });

            return listaTipoDeUsuario;
        }

        [AllowAnonymous]
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> EsqueciSenha(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                ViewBag.Message = "Se o e-mail estiver cadastrado, você receberá um link para redefinir sua senha.";
                return View();
            }

            // Gere um token simples (exemplo: Guid + validade)
            var token = Guid.NewGuid().ToString();
            usuario.ResetToken = token;
            usuario.ResetTokenValidade = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();


            var resetLink = Url.Action("RedefinirSenha", "Usuarios", new { email = usuario.Email, token = token }, Request.Scheme);

            // Envie o e-mail (substitua por seu serviço de e-mail real)
            // await _emailSender.SendEmailAsync(usuario.Email, "Redefinição de senha", $"Clique aqui para redefinir: {resetLink}");
            _logger.LogInformation("Link de redefinição: {resetLink}", resetLink);

            ViewBag.Message = "Se o e-mail estiver cadastrado, você receberá um link para redefinir sua senha.";
            return View();
        }

        [AllowAnonymous]
        public IActionResult RedefinirSenha(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RedefinirSenha(string email, string token, string novaSenha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null || usuario.Senha == null)
            {
                ViewBag.Message = "Link inválido ou expirado.";
                return View();
            }

            if (usuario.ResetToken != token || usuario.ResetTokenValidade == null || usuario.ResetTokenValidade < DateTime.UtcNow)
            {
                ViewBag.Message = "Link inválido ou expirado.";
                return View();
            }

            // Atualiza senha e limpa o token
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
            usuario.ResetToken = null;
            usuario.ResetTokenValidade = null;
            await _context.SaveChangesAsync();


            ViewBag.Message = "Senha redefinida com sucesso! Você já pode fazer login.";
            return View();
        }


    }
}
