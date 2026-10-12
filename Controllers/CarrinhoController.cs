using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
