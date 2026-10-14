using Discoffee.Models;
using Discoffee.ViewModels;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using Discoffee.Sessao;
using ZstdSharp.Unsafe;

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

        // [HttpPost]
        // public IActionResult FinalizarCompra(Pedido pedido)
        // {
        //     // 1. Criar o pedido
        //     int idPedido = _pedidoRepository.CriarPedido(pedido);

        //     // 2. Percorrer os produtos do carrinho
        //     foreach (var item in itens)
        //     {
        //         // 3. Inserir o produto e capturar seu Id_Item
        //         int idItem = _pedidoRepository.InserirItemPedido(
        //             idPedido,
        //             item.IdProduto,
        //             item.Quantidade
        //         );

        //         // 4. Inserir as personalizações desse produto
        //         foreach (var idOpcao in item.Opcoes)
        //         {
        //             _pedidoRepository.InserirOpcaoItemPedido(idItem, idOpcao);
        //         }
        //     }

        //     // 5. Recalcular o subtotal e o total do pedido
        //     _pedidoRepository.AtualizarTotalPedido(idPedido);
        // }
    }
}
