using Discoffee.Models;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    [Authorize(Roles = "Administrador, Funcionario")]
    public class DashboardController : Controller
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICupomRepository _cupomRepository;
        public DashboardController(IPagamentoRepository pagamentoRepository, IPedidoRepository pedidoRepository,
            IUsuarioRepository usuarioRepository, IProdutoRepository produtoRepository,ICupomRepository cupomRepository)
        {
            _pagamentoRepository = pagamentoRepository;
            _pedidoRepository = pedidoRepository;
            _cupomRepository = cupomRepository;
            _usuarioRepository = usuarioRepository;
            _produtoRepository = produtoRepository;
        }       

        public IActionResult Index()
        {
            List<Pagamento> pagamentos = _pagamentoRepository.VerTodos();
            List<PedidoAdminViewModel> pedidos = _pedidoRepository.BuscarTodos();
            List<Produto> produtos = _produtoRepository.BuscarTodos();
            List<Usuario> usuarios = _usuarioRepository.BuscarTodos();

            DashboardViewModel dashboard = new DashboardViewModel
            {
                pagamentos = pagamentos,
                pedidos = pedidos,
                produtos = produtos,
                usuarios = usuarios
            };

            return View(dashboard);
        }
    }
}