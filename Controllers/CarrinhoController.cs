using Discoffee.Models;
using Discoffee.ViewModels;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using Discoffee.Sessao;
using ZstdSharp.Unsafe;
using Microsoft.AspNetCore.Http.Features;

namespace Discoffee.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly SessaoItemPedido _sessaoItemPedido;

        public CarrinhoController(IPedidoRepository pedidoRepository, SessaoItemPedido sessaoItemPedido)
        {
            _pedidoRepository = pedidoRepository;
            _sessaoItemPedido = sessaoItemPedido;
             
        }

        public IActionResult Index()
        {
            var itens = _sessaoItemPedido.Consultar();

            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(ItemPedidoViewModel item)
        {
            _sessaoItemPedido.Cadastrar(item);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Atualizar(ItemPedidoViewModel item)
        {
            _sessaoItemPedido.Atualizar(item);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remover(ItemPedidoViewModel item)
        {
            _sessaoItemPedido.Remover(item);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public void FinalizarCompra([FromBody] Pedido pedido)
        {
            int idPedido = _pedidoRepository.CriarPedido(pedido);

            // PASSA POR CADA UM DOS PRODUTOS NO CARRINHO
            foreach (var item in pedido.Itens)
            {
                var itemPedido = new ItemPedidoViewModel
                {
                    IdPedido = idPedido,
                    IdProduto = item.IdProduto,
                    Quantidade = item.Quantidade
                };

                // INSERE O PRODUTO E PEGA SEU ID
                int idItem = _pedidoRepository.InserirItemPedido(
                    itemPedido
                );

                // INSERE AS PERSONALIZAÇÕES DESSE PRODUTO
                foreach (var opcao in item.Opcoes)
                {
                    var opcaoItem = new OpcaoPedidoViewModel
                    {
                        IdItem = idItem,
                        IdOpcao = opcao.IdOpcao
                    };

                    _pedidoRepository.InserirOpcaoItemPedido(opcaoItem);
                }
            }

            // RECALCULA O TOTAL
            _pedidoRepository.AtualizarTotalPedido(idPedido);
        }
    }
}
