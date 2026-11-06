using System.Security.Claims;
using Discoffee.Models;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize(Roles = "Cliente, Administrador")]
    public class CartaoController : Controller
    {
        private readonly ICartaoRepository _cartaoRepository;
        
        public CartaoController(ICartaoRepository cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
        }

        [HttpPost]
        public IActionResult AdicionarCartao(Cartao cartao)
        {
            var idUsuario = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            _cartaoRepository.AdicionarCartao(cartao, idUsuario);
            return RedirectToAction("Index", "Perfil", new { area = "Cliente" });
        }
    }
}