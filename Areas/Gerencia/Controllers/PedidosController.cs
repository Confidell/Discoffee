using Discoffee.Areas.Cliente.Controllers;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    [Authorize(Roles = "Administrador, Funcionario")]
    public class PedidosController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidosController (IPedidoRepository pedidoRepository){
            _pedidoRepository = pedidoRepository;
        }

        public IActionResult Index()
        {
            var pedidos = _pedidoRepository.BuscarTodos();
            return View(pedidos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AlterarStatus(int idPedido, StatusPedido status)
        {
            _pedidoRepository.AlterarStatusPedido(idPedido, status);
            return RedirectToAction(nameof(Index));
        }

    }
}