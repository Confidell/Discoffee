using Discoffee.Models;
using Discoffee.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize(Roles = "Cliente")]
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            var pedidos = new List<Pedido>
            {
                new Pedido
                {
                    IdPedido = 1010,
                    Total = 41.90m,
                    Status = StatusPedido.Entregue,
                    QuantidadeProduto = 2,
                    DataHora = new DateTime(2026, 7, 20)
                },
                new Pedido
                {
                    IdPedido = 1011,
                    Total = 12.50m,
                    Status = StatusPedido.Cancelado,
                    QuantidadeProduto = 1,
                    DataHora = new DateTime(2026, 7, 23)
                },
                new Pedido
                {
                    IdPedido = 1012,
                    Total = 50.20m,
                    Status = StatusPedido.EmPreparo,
                    QuantidadeProduto = 5,
                    DataHora = new DateTime(2026, 7, 24)
                },
                                new Pedido
                {
                    IdPedido = 1013,
                    Total = 50.20m,
                    Status = StatusPedido.EmPreparo,
                    QuantidadeProduto = 5,
                    DataHora = new DateTime(2026, 7, 24)
                },
                                new Pedido
                {
                    IdPedido = 1014,
                    Total = 50.20m,
                    Status = StatusPedido.EmPreparo,
                    QuantidadeProduto = 5,
                    DataHora = new DateTime(2026, 7, 24)
                },
                                new Pedido
                {
                    IdPedido = 1015,
                    Total = 50.20m,
                    Status = StatusPedido.EmPreparo,
                    QuantidadeProduto = 5,
                    DataHora = new DateTime(2026, 7, 24)
                }
            };

            return View(pedidos);
        }
    }
}