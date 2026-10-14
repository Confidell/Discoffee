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
            return View();
        }
    }
}