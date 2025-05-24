using System.ComponentModel.DataAnnotations;

namespace click_imoveis.Models
{
    public class RelatorioViewModel
    {
        // Totais gerais
        public int TotalImoveis { get; set; }
        public int TotalAnuncios { get; set; }
        public int TotalUsuarios { get; set; }

        // Desempenho por imóvel
        public List<DesempenhoImovel> DesempenhoImoveis { get; set; } = new();

        // Tendência de preço
        public List<TendenciaPreco> TendenciaPrecos { get; set; } = new();
    }

    public class DesempenhoImovel
    {
        public int ImovelId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int TotalVisualizacoes { get; set; }
        public int TotalMensagens { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class TendenciaPreco
    {
        public string Local { get; set; } = string.Empty;
        public decimal MediaPreco { get; set; }
        public int TotalImoveis { get; set; }
    }
} 
