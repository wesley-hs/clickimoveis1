using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Anuncios")]
    public class Anuncio
    {
        [Key]
        public int AnuncioId { get; set; }

        [Display(Name = "Data de Início")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        public DateTime? DataFim { get; set; }
        public float? Valor { get; set; }

        [Display(Name = "Valor do Condomínio")]
        public float? ValorCondominio { get; set; }

        [Display(Name = "Valor do IPTU")]
        public float? ValorIptu { get; set; }

        [Display(Name = "Título")]
        public string? Titulo { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }
        public Finalidade Finalidade { get; set; }



        public int? ImovelId { get; set; }
        public Imovel? Imovel { get; set; }

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<Mensagem>? Mensagens { get; set; }

    }

    public enum Finalidade
    {
        Locação,
        Venda,
        Temporada
    }
}
