using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace click_imoveis.Models
{
    [Table("PessoasFisicas")]
    public class PessoaFisica
    {
        [Key]
        public int PessoaFisicaId { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório!")]
        public required string NomeCompleto { get; set; }

        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        [Display(Name = "Data de Nascimento")]
        public string? DataNascimento { get; set; }

                
        public int UsuarioId { get; set; }
                
        public Usuario? Usuario { get; set; }
    }
}
