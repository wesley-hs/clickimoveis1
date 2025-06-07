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

        [Required(ErrorMessage = "O comentário é obrigatório.")]
        [MinLength(5, ErrorMessage = "O comentário deve ter pelo menos 5 caracteres.")]
        public string? Conteudo { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5.")]
        public short? Nota { get; set; }


        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int ImovelId { get; set; }
        public Imovel? Imovel { get; set; }
        public int? ComentarioPaiId { get; internal set; }
    }
}
