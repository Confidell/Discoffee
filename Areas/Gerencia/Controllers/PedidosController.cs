using Discoffee.Areas.Cliente.Controllers;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
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

        // [HttpGet]
        // public IActionResult BuscarTodosPedidos()
        // {
        //     var pedidos = _pedidoRepository.BuscarTodos();
        //     return Ok(pedidos);
        // }

    }
}