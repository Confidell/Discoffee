using Discoffee.Models.Constants;

namespace Discoffee.Repository
{
    public class Pedido
    {
        public Pedido BuscarPedidoPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarPedidoPorStatus(StatusPedido status)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarPedidoPorTipo(TipoPedido tipo)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarPedidosPorUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public void CancelarPedido(int id)
        {
            throw new NotImplementedException();
        }

        public void ConfirmarPedido(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        public void EditarPedido(Pedido pedido)
        {
            throw new NotImplementedException();
        }
    }
}
