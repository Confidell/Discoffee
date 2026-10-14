using Discoffee.Models;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        public CarrinhoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public IActionResult Index()
        {
            return View();
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
