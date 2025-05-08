using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace click_imoveis.Models
{
    [Table("Midias")]
    public class Midia
    {
        [Key]
        public int MidiaId { get; set; }
        public string Link { get; set; }
        public TipoDeMidia TipoDeMidia { get; set; }


        public int? ImovelId { get; set; }
        public Imovel? Imovel { get; set; }
    }

    public enum TipoDeMidia
    {
        Imagem = 1,
        Video = 2,
        Audio = 3
    }
}
