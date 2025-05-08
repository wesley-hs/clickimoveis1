using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Imoveis")]
    public class Imovel
    {
        [Key]
        public int ImovelId { get; set; }
        public int Quartos { get; set; }
        public int Banheiros { get; set; }


        //Esta propriedade de navegação é necessária para o Entity Framework
        //para criar a relação entre Imovel e Usuario um-para-muitos
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<Comentario>? Comentarios { get; set; }
        public ICollection<Anuncio>? Anuncios { get; set; }
        public ICollection<Midia>? Midias { get; set; }
    }
}
