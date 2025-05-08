using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Comentarios")]
    public class Comentario
    {
        [Key]
        public int ComentarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Conteudo { get; set; }
        public string Nota { get; set; }


        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int ImovelId { get; set; }
        public Imovel? Imovel { get; set; }
    }
}
