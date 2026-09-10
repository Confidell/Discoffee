using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}