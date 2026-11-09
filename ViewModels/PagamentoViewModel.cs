using Discoffee.Models;

namespace Discoffee.ViewModels
{
    public class PagamentoViewModel
    {
        public List<ItemPedidoViewModel>? Itens { get; set; }

        public Usuario Usuario { get; set; }

        public List<Cartao>? Cartoes { get; set; }
    }
}