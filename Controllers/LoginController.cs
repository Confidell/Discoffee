using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            // Validação simples de campos vazios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                ViewBag.MensagemErro = "Por favor, preencha todos os campos.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
    