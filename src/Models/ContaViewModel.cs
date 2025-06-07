using click_imoveis.Controllers;

namespace click_imoveis.Models
{
    public class ContaViewModel
    {
        public Usuario? Usuario { get; set; }
        public PessoaFisica? PessoaFisica { get; set; }
        public PessoaJuridica? PessoaJuridica { get; set; }
        public ICollection<Imovel>? Imoveis { get; set; }
        public ICollection<Anuncio>? Anuncios { get; set; }
        public IFormFile? FotoUpload { get; set; }

    }
}
