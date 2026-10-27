using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.ViewModels;

namespace Discoffee.Repository.Contract
{
    public interface IPedidoRepository
    {   
        int CriarPedido(Pedido pedido);
        // void EditarPedido(Pedido pedido);
        void AlterarStatusPedido(int IdPedido, StatusPedido status);
        int InserirItemPedido(ItemPedidoViewModel itemPedido);
        public void InserirOpcaoItemPedido(OpcaoPedidoViewModel opcaoPedido);
        public void AtualizarTotalPedido(int IdPedido);
        List<PedidoClienteViewModel> BuscarPedidosPorUsuario(int IdUsuario);
        List<PedidoAdminViewModel> BuscarTodos();
    }
}
