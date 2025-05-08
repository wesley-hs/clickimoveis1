using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Anuncios")]
    public class Anuncio
    {
        [Key]
        public int AnuncioId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public float? Valor { get; set; }
        public float? ValorCondominio { get; set; }
        public float? ValorIptu { get; set; }
        public string? Titulo { get; set; }
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
