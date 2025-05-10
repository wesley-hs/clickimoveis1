using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        
        [Required(ErrorMessage = "Obrigatório informar email.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório informar senha.")]
        public required string Senha { get; set; }

        [Display(Name = "Tipo de Usuário")]
        [Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório.")]
        public TipoDeUsuario TipoDeUsuario { get; set; }

        public string? Creci { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        public string? Logradouro { get; set; }

        [Display(Name = "Número")]
        public string? Numero { get; set; }

        public string? Complemento { get; set; }

        public string? Bairro { get; set; }

        public string? Estado { get; set; }

        [Display(Name = "CEP")]
        public string? CodigoPostal { get; set; }

        public string? TelefonePrincipal { get; set; }

        public string? TelefoneAlternativo1 { get; set; }

        public string? TelefoneAlternativo2 { get; set; }


        [Display(Name = "Pessoa Física / Pessoa Jurídica")]
        [Required(ErrorMessage = "O campo Pessoa é obrigatório.")]
        public Pessoa Pessoa { get; set; }

        


        public ICollection<Imovel>? Imoveis { get; set; }
        public ICollection<Mensagem>? Mensagens { get; set; }
        public ICollection<Anuncio>? Anuncios { get; set; }
        public ICollection<Comentario>? Comentarios { get; set; }

        // One to one-or-zero relationship
        public PessoaFisica? PessoaFisica { get; set; }
        public PessoaJuridica? PessoaJuridica { get; set; }
            

    }

    public enum TipoDeUsuario
    {
        Corretor,
        Imobiliária,
        Usuário,
    }
    public enum Pessoa
    {
        PessoaFisica,
        PessoaJuridica
    }
}
