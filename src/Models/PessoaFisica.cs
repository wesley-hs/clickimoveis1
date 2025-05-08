using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("PessoasFisicas")]
    public class PessoaFisica
    {
        [Key]
        public int PessoaFisicaId { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório!")]
        public required string NomeCompleto { get; set; }

        public int UsuarioId { get; set; }
        public required Usuario Usuario { get; set; }
    }
}
