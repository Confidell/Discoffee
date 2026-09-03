using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Carrinho
    {
        public int IdCarrinho { get; set; }

        public Produto Produto { get; set; }

        [Required]
        public decimal PrecoUnitario { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public decimal Total { get; set; }

    }
}
