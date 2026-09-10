using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class PerfilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}