using System.Security.Claims;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize(Roles = "Cliente")]
    public class PerfilController : Controller
    {
        private readonly IUsuarioRepository _repositorio;
        public PerfilController(IUsuarioRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public IActionResult Index()
        {
            // PEGA O ID DO USUÁRIO AUTENTICADO SALVO NOS COOKIES
            var idUsuario = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
            
            var usuario = _repositorio.BuscarUsuarioPorId(idUsuario);

            if(usuario == null)
                return NotFound();

            return View(usuario);
        }
    }
}