using System.Security.Claims;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.Sessao;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize(Roles = "Cliente")]
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly SessaoItemPedido _sessaoItemPedido;
        private readonly IProdutoRepository _produtoRepository;

        public PedidoController(IPedidoRepository pedidoRepository, SessaoItemPedido sessaoItemPedido, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _sessaoItemPedido = sessaoItemPedido;
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        {
            int IdUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var pedidos = _pedidoRepository.BuscarPedidosPorUsuario(IdUsuario);
            return View(pedidos);
        }

        [HttpPost]
        public IActionResult FinalizarCompra(Pedido pedido)
        {

            // RECUPERA OS ITENS DO CARRINHO NA SESSÃO
            var itensCarrinho = _sessaoItemPedido.Consultar();

            if (itensCarrinho.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            // SE ALGUM PRODUTO FOI DESATIVADO, NÃO FINALIZA: VOLTA AO CARRINHO, QUE REMOVE O ITEM E AVISA O CLIENTE
            bool temIndisponivel = itensCarrinho.Any(item =>
            {
                var produto = item.IdProduto is int idProduto
                    ? _produtoRepository.BuscarProdutoPorId(idProduto)
                    : null;

                return produto == null || !produto.Disponibilidade;
            });

            if (temIndisponivel)
            {
                return RedirectToAction("Index", "Carrinho", new { area = "" });
            }

            // RECUPERA O ID DO USUÁRIO AUTENTICADO
            var idUsuarioClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return Unauthorized();
            }

            // DEFINE O USUÁRIO DA COMPRA
            pedido.Usuario = new Usuario
            {
                IdUsuario = idUsuario
            };

            // CRIA O PEDIDO NO BANCO
            int idPedido = _pedidoRepository.CriarPedido(pedido);

            // PERCORRE OS PRODUTOS DO CARRINHO
            foreach (var item in itensCarrinho)
            {
                var itemPedido = new ItemPedidoViewModel
                {
                    IdPedido = idPedido,
                    IdProduto = item.IdProduto,
                    Quantidade = item.Quantidade
                };

                // INSERE O PRODUTO E RECUPERA O ID DO ITEM
                int idItem = _pedidoRepository.InserirItemPedido(
                    itemPedido
                );

                // INSERE AS PERSONALIZAÇÕES DO PRODUTO
                foreach (var opcao in item.Opcoes)
                {
                    var opcaoPedido = new OpcaoPedidoViewModel
                    {
                        IdItem = idItem,
                        IdOpcao = opcao.IdOpcao
                    };

                    _pedidoRepository.InserirOpcaoItemPedido(
                        opcaoPedido
                    );
                }
            }

            // RECALCULA O TOTAL DO PEDIDO
            _pedidoRepository.AtualizarTotalPedido(idPedido);

            // LIMPA O CARRINHO APÓS CONCLUIR A COMPRA
            _sessaoItemPedido.Limpar();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CancelarPedido(int IdPedido)
        {   
            _pedidoRepository.AlterarStatusPedido(IdPedido, StatusPedido.Cancelado);
            return RedirectToAction(nameof(Index));
        }
    }
}