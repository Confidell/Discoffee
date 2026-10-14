using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.ViewModels;

namespace Discoffee.Repository.Contract
{
    public interface IPedidoRepository
    {   
        int CriarPedido(Pedido pedido);
        void EditarPedido(Pedido pedido);
        void CancelarPedido(int id);
        int InserirItemPedido(ItemPedidoViewModel itemPedido);
        public void InserirOpcaoItemPedido(OpcaoPedidoViewModel opcaoPedido);
        public void AtualizarTotalPedido(int IdPedido);
        Pedido BuscarPedidoPorId(int id);
        List<Pedido> BuscarPedidosPorUsuario(int usuarioId);
        List<Pedido> BuscarTodos();
        List<Pedido> BuscarPedidoPorTipo(TipoPedido tipo);
        List<Pedido> BuscarPedidoPorStatus(StatusPedido status);
    }
}
