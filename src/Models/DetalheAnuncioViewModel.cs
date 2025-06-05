namespace click_imoveis.Models
{
    public class DetalheAnuncioViewModel
    {
        public Anuncio anuncio { get; set; }
        public Mensagem mensagem { get; set; }
        public Comentario comentario { get; set; }
        public Imovel imovel { get; set; }
    }
}
