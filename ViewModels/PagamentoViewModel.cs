using Discoffee.Models;

namespace Discoffee.ViewModels
{
    public class PagamentoViewModel
    {
        public int IdPedido { get; set; }

        public string? Cupom { get; set; }
        
        public PedidoClienteViewModel Pedido { get; set; }

        public Usuario Usuario { get; set; }

        public List<Cartao>? Cartoes { get; set; }
    }
}