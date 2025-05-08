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
        public string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório informar senha.")]
        public string Senha { get; set; }

        [Display(Name = "Tipo de Usuário")]
        [Required(ErrorMessage = "O campo Tipo de Usuário é obrigatório.")]
        public TipoDeUsuario TipoDeUsuario { get; set; }

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

        //public ICollection<PessoaFisica>? PessoasFisicas { get; set; } = new List<PessoaFisica> { };
        //public ICollection<PessoaJuridica>? PessoasJuridicas { get; set; } = new List<PessoaJuridica> { };


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
