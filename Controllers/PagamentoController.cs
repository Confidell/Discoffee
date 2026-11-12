using System.Security.Claims;
using Discoffee.Models;
using Discoffee.Repository;
using Discoffee.Repository.Contract;
using Discoffee.Sessao;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{   
    [Authorize(Roles = "Cliente, Administrador, Funcionario")]
    public class PagamentoController : Controller
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly SessaoItemPedido _sessaoItemPedido;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public PagamentoController(IPagamentoRepository repository, IUsuarioRepository usuarioRepository, 
                ICartaoRepository cartaoRepository, IPedidoRepository pedidoRepository
            )
        {
            _pagamentoRepository = repository;
            _usuarioRepository = usuarioRepository;
            _cartaoRepository = cartaoRepository;
            _pedidoRepository = pedidoRepository;
        }        

        public IActionResult Index(int idPedido, string? cupom)
        {
            var idUsuario = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            ); 

            PagamentoViewModel pagamento = new PagamentoViewModel
            {
                IdPedido = idPedido,
                Cupom = cupom,
                Pedido = _pedidoRepository.BuscarPedidoPorId(idPedido),
                Usuario = _usuarioRepository.BuscarUsuarioPorId(idUsuario),
                Cartoes = _cartaoRepository.BuscarTodosPorUsuario(idUsuario)
            };

            return View(pagamento);
        }

        // [HttpPost]
        // public IActionResult RegistrarPagamento(Pagamento pagamento)
        // {
            
        // }
    }
}