using System.Security.Claims;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize(Roles = "Cliente")]
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public IActionResult Index()
        {
            int IdUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var pedidos = _pedidoRepository.BuscarPedidosPorUsuario(IdUsuario);
            return View(pedidos);
        }

        [HttpPost]
        public IActionResult CancelarPedido(int IdPedido)
        {   
            _pedidoRepository.AlterarStatusPedido(IdPedido, StatusPedido.Cancelado);
            return RedirectToAction(nameof(Index));
        }
    }
}