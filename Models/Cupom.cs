using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Cupom
    {
        public int IdCupom { get; set; }

        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataValidade { get; set; }

        [Required]
        public decimal Desconto { get; set; }

    }
}
