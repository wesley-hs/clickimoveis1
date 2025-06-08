using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Mensagens")]
    public class Mensagem
    {
        [Key]
        public int MensagemId { get; set; }
        public DateTime DataCriacao { get; set; }
        public required string Conteudo { get; set; }


        

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int? AnuncioId { get; set; }
        public Anuncio? Anuncio { get; set; }
    }
}
