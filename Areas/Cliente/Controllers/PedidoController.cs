using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}