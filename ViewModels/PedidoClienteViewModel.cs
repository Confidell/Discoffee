using Discoffee.Models;
using Discoffee.Models.Constants;

namespace Discoffee.ViewModels
{
    public class PedidoClienteViewModel
    {
        public int IdPedido { get; set; }
        public DateTime DataHora { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Frete { get; set; }
        public decimal Total { get; set; }
        public StatusPedido Status { get; set; }
        public TipoPedido Tipo { get; set; }
        public int IdUsuario { get; set; }
        public List<ItemPedidoViewModel> Itens { get; set; } = new();
        public Pagamento? Pagamento { get; set; } = new();
    }
}