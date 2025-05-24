using click_imoveis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace click_imoveis.Controllers
{
    public class RelatoriosController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

       
        public IActionResult Index()
        {
            var desempenhoImoveis = _context.Anuncios
                .Include(a => a.Imovel)
                .Include(a => a.Mensagens)
                .Select(a => new DesempenhoImovel
                {
                    ImovelId = a.ImovelId ?? 0,
                    Titulo = a.Titulo ?? "(Sem título)",
                    TotalVisualizacoes = a.TotalVisualizacoes,
                    TotalMensagens = a.Mensagens != null ? a.Mensagens.Count : 0,
                    DataCadastro = a.DataInicio ?? DateTime.MinValue
                })
                .ToList();

            var tendenciaPrecos = _context.Anuncios
                .Include(a => a.Imovel)
                .Where(a => a.Imovel != null && a.Imovel.Bairro != null && a.Valor != null)
                .GroupBy(a => a.Imovel.Bairro)
                .Select(g => new TendenciaPreco
                {
                    Local = g.Key!,
                    MediaPreco = (decimal)g.Average(a => a.Valor ?? 0),
                    TotalImoveis = g.Count()
                })
                .ToList();

            var model = new RelatorioViewModel
            {
                TotalImoveis = _context.Imoveis.Count(),
                TotalAnuncios = _context.Anuncios.Count(),
                TotalUsuarios = _context.Usuarios.Count(),
                DesempenhoImoveis = desempenhoImoveis,
                TendenciaPrecos = tendenciaPrecos
            };

            return View(model);
        }
    }
}
