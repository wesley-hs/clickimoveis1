using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("PessoasJuridicas")]
    public class PessoaJuridica
    {
        [Key]
        public int PessoaJuridicaId { get; set; }

        [Display(Name ="Razão Social")]
        [Required(ErrorMessage = "O campo Razão Social é obrigatório.")]
        public required string RazaoSocial { get; set; }

        [Display(Name = "CNPJ")]
        public string? Cnpj { get; set; }

        [Display(Name = "Nome Fantasia")]
        public string? NomeFantasia { get; set; }



        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

    }
    
    
    
}
