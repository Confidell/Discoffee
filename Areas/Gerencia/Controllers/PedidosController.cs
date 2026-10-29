using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    public class PedidosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // [HttpGet]
        // public IActionResult BuscarTodosPedidos()
        // {
        //     var pedidos = _pedidoRepository.BuscarTodos();
        //     return Ok(pedidos);
        // }
    }
}