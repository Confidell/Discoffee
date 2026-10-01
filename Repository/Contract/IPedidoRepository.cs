using Discoffee.Models.Constants;

namespace Discoffee.Repository.Contract
{
    public interface IPedidoRepository
    {
        void ConfirmarPedido(PedidoRepository pedido);
        void EditarPedido(PedidoRepository pedido);
        void CancelarPedido(int id);
        PedidoRepository BuscarPedidoPorId(int id);
        List<PedidoRepository> BuscarPedidosPorUsuario(int usuarioId);
        List<PedidoRepository> BuscarTodos();
        List<PedidoRepository> BuscarPedidoPorTipo(TipoPedido tipo);
        List<PedidoRepository> BuscarPedidoPorStatus(StatusPedido status);
    }
}
