using Discoffee.Models;
using Discoffee.ViewModels;

namespace Discoffee.Repository.Contract
{
    public interface IPedidoRepository
    {   
        int CriarPedido(Pedido pedido);
        void EditarPedido(Pedido pedido);
        void CancelarPedido(int IdPedido);
        int InserirItemPedido(ItemPedidoViewModel itemPedido);
        public void InserirOpcaoItemPedido(OpcaoPedidoViewModel opcaoPedido);
        public void AtualizarTotalPedido(int IdPedido);
        List<Pedido> BuscarPedidosPorUsuario(int IdUsuario);
        List<Pedido> BuscarTodos();
    }
}
