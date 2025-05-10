using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Imoveis")]
    public class Imovel
    {
        [Key]
        public int ImovelId { get; set; }
        public int? Quartos { get; set; }
        public int? Banheiros { get; set; }
        public int? Metragem { get; set; }

        public int? Vagas { get; set; }

        public int? Suites { get; set; }

        public string? Logradouro { get; set; }

        public string? Numero { get; set; }

        public string? Complemento { get; set; }

        public string? Bairro { get; set; }

        public string? Cidade { get; set; }

        public string? Estado { get; set; }

        [Display(Name = "CEP")]
        public string? CodigoPostal { get; set; }



        //Esta propriedade de navegação é necessária para o Entity Framework
        //para criar a relação entre Imovel e Usuario um-para-muitos
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<Comentario>? Comentarios { get; set; }
        public ICollection<Anuncio>? Anuncios { get; set; }
        public ICollection<Midia>? Midias { get; set; }
    }
}
