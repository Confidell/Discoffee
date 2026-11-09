using System.Security.Claims;
using Discoffee.Repository.Contract;
using Discoffee.Sessao;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class PagamentoController : Controller
    {
        // private readonly IPagamentoRepository _pagamentoRepository;
        private readonly SessaoItemPedido _sessaoItemPedido;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICartaoRepository _cartaoRepository;
        public PagamentoController(
            // IPagamentoRepository repository
                IUsuarioRepository usuarioRepository,
                SessaoItemPedido sessaoItemPedido,
                ICartaoRepository cartaoRepository
            )
        {
            // _pagamentoRepository = repository;
            _sessaoItemPedido = sessaoItemPedido;
            _usuarioRepository = usuarioRepository;
            _cartaoRepository = cartaoRepository;
        }        

        public IActionResult Index()
        {
            var idUsuario = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            PagamentoViewModel pagamento = new PagamentoViewModel
            {
                Itens = _sessaoItemPedido.Consultar(),
                Usuario = _usuarioRepository.BuscarUsuarioPorId(idUsuario),
                Cartoes = _cartaoRepository.BuscarTodosPorUsuario(idUsuario)
            };

            return View(pagamento);
        }


    }
}