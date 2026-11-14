using System.Security.Claims;
using Discoffee.Models;
using Discoffee.Models.Constants;
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

            var pedido = _pedidoRepository.BuscarPedidoPorId(idPedido);
            if(pedido.Pagamento is not null && pedido.Pagamento?.Status == EnumStatusPagamento.Pago)
            {
                ModelState.AddModelError("Pedido", "Esse pedido já foi pago!");
                return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
            }

            if(pedido.Status == StatusPedido.Cancelado || pedido.Pagamento?.Status == EnumStatusPagamento.Recusado)
            {
                ModelState.AddModelError("Pedido", "Esse pedido foi cancelado!");
                return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
            }

            PagamentoViewModel pagamento = new PagamentoViewModel
            {
                IdPedido = idPedido,
                Cupom = cupom,
                Pedido = pedido,
                Usuario = _usuarioRepository.BuscarUsuarioPorId(idUsuario),
                Cartoes = _cartaoRepository.BuscarTodosPorUsuario(idUsuario)
            };

            return View(pagamento);
        }

        [HttpPost]
        public IActionResult EfetuarPagamento(Pagamento pagamento)
        {
            if (pagamento.Pedido == null)
            {
                ModelState.AddModelError("Erro", "Pedido não informado!");
                return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
            }

            var pedidoExistente = _pedidoRepository.BuscarPedidoPorId(
                pagamento.Pedido.IdPedido
            );

            if (pedidoExistente is null)
            {
                ModelState.AddModelError("Erro", "Esse pedido não existe!");
                return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
            }

            if (pagamento.FormaPagamento == EnumFormaPagamento.Cartao &&
                (pagamento.Cartao == null || pagamento.Cartao.IdCartao <= 0))
            {
                ModelState.AddModelError("Erro", "Nenhum cartão selecionado!");
                return RedirectToAction("Index", new { pagamento.Pedido.IdPedido });
            }
            
            // FINALIZA O PAGAMENTO
            _pagamentoRepository.EfetuarPagamento(pagamento);
            // ATUALIZA O STATUS DO PEDIDO DE PENDENTE PARA "PREPARANDO"
            _pedidoRepository.AlterarStatusPedido(pagamento.Pedido.IdPedido, StatusPedido.Preparando);

            return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
        }

        [HttpPost]
        public IActionResult RecusarPagamento(Pagamento pagamento)
        {
            if (pagamento.Pedido == null)
            {
                ModelState.AddModelError("Erro", "Pedido não informado!");
                return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
            }

            var pedidoExistente = _pedidoRepository.BuscarPedidoPorId(
                pagamento.Pedido.IdPedido
            );

            if (pedidoExistente is null)
            {
                ModelState.AddModelError("Erro", "Esse pedido não existe!");
                return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
            }

            if (pagamento.FormaPagamento == EnumFormaPagamento.Cartao &&
                (pagamento.Cartao == null || pagamento.Cartao.IdCartao <= 0))
            {
                ModelState.AddModelError("Erro", "Nenhum cartão selecionado!");
                return RedirectToAction("Index", new { pagamento.Pedido.IdPedido });
            }

            _pagamentoRepository.EfetuarPagamento(pagamento);
            _pedidoRepository.AlterarStatusPedido(pagamento.Pedido.IdPedido, StatusPedido.Cancelado);

            return RedirectToAction("Index", "Pedido", new { area = "Cliente" });
        }
    }
}