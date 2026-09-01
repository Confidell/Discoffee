using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }

        [Required]
        public DateTime DataHora { get; set; }
        public Produto Produto { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public int QuantidadeProduto { get; set; }

        public StatusPedido Status { get; set; }
        public TipoPedido Tipo { get; set; }
    }
}
