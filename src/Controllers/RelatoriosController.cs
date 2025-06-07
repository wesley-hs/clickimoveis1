using click_imoveis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace click_imoveis.Controllers
{
    public class RelatoriosController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;
        private List<string> cidades;

        public IActionResult Index(int pagina = 1, int tamanhoPagina = 10)
        {
            // Lista de cidades para o filtro
            var cidades = _context.Imoveis
                .Where(i => i.Cidade != null && i.Cidade != "")
                .Select(i => i.Cidade)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            // Consulta base dos anúncios
            var anunciosQuery = _context.Anuncios
                .Include(a => a.Imovel)
                .Include(a => a.Mensagens)
                .OrderByDescending(a => a.DataInicio)
                .AsQueryable();

            // Paginação
            int totalAnuncios = anunciosQuery.Count();
            int totalPaginas = (int)Math.Ceiling(totalAnuncios / (double)tamanhoPagina);

            var desempenhoImoveis = anunciosQuery
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .Select(a => new DesempenhoImovel
                {
                    ImovelId = a.ImovelId ?? 0,
                    Titulo = a.Titulo ?? "(Sem título)",
                    TotalVisualizacoes = a.TotalVisualizacoes,
                    TotalMensagens = a.Mensagens != null ? a.Mensagens.Count : 0,
                    DataCadastro = a.DataInicio ?? DateTime.MinValue
                })
                .ToList();

            // Tendência de preços (mantém igual)
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
                TendenciaPrecos = tendenciaPrecos,
                Cidades = cidades,
                PaginaAtual = pagina,
                TotalPaginas = totalPaginas
            };

            return View(model);
        }
        private List<DesempenhoImovel> ObterDesempenhoImoveisPaginado(int pagina, int tamanhoPagina)
        {
            var anunciosQuery = _context.Anuncios
                .Include(a => a.Imovel)
                .Include(a => a.Mensagens)
                .OrderByDescending(a => a.DataInicio)
                .AsQueryable();

            return anunciosQuery
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .Select(a => new DesempenhoImovel
                {
                    ImovelId = a.ImovelId ?? 0,
                    Titulo = a.Titulo ?? "(Sem título)",
                    TotalVisualizacoes = a.TotalVisualizacoes,
                    TotalMensagens = a.Mensagens != null ? a.Mensagens.Count : 0,
                    DataCadastro = a.DataInicio ?? DateTime.MinValue
                })
                .ToList();
        }

        [HttpGet]
        public IActionResult CarregarMaisImoveis(int pagina = 1, int tamanhoPagina = 10)
        {
            var imoveis = ObterDesempenhoImoveisPaginado(pagina, tamanhoPagina);
            return PartialView("_TabelaImoveisPartial", imoveis);
        }


    }
}
