using Discoffee.Models;

namespace Discoffee.ViewModels
{
    public class DashboardViewModel
    {
        public List<Pagamento> pagamentos = new();
        public List<PedidoAdminViewModel> pedidos = new();
        public List<Produto> produtos = new();
        public List<Usuario> usuarios = new();
        public List<Cupom> cupom = new();
    }
}