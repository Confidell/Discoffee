using Discoffee.Models.Constants;

namespace Discoffee.Repository.Contract
{
    public interface IPedido
    {
        void ConfirmarPedido(Pedido pedido);
        void EditarPedido(Pedido pedido);
        void CancelarPedido(int id);
        Pedido BuscarPedidoPorId(int id);
        List<Pedido> BuscarPedidosPorUsuario(int usuarioId);
        List<Pedido> BuscarTodos();
        List<Pedido> BuscarPedidoPorTipo(TipoPedido tipo);
        List<Pedido> BuscarPedidoPorStatus(StatusPedido status);
    }
}
