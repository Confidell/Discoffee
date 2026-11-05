using System.Security.Claims;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize(Roles = "Cliente, Administrador, Funcionario")]
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

            var viewModel = new EditarUsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                // Senha = usuario.Senha,
                CPF = usuario.CPF,
                Telefone = usuario.Telefone,
                DataNascimento = usuario.DataNascimento,
                Status = usuario.Status,
                Complemento = usuario.Complemento,
                NumeroCasa = usuario.NumeroCasa,
                CEP = usuario.CEP,
                Perfil = usuario.Perfil
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditarCliente(EditarUsuarioViewModel usuario)
        {
            usuario.IdUsuario = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var usuarioEmail = _repositorio.BuscarUsuarioPorEmail(usuario.Email);

            if (usuarioEmail is not null && usuarioEmail.IdUsuario != usuario.IdUsuario)
            {
                ModelState.AddModelError(
                    "Email",
                    "Este Email já pertence a um usuário."
                );

                return View("Index", usuario);
            }
            
            var usuarioCPF = _repositorio.BuscaUsuarioPorCPF(usuario.CPF);

            // VERIFICA SE JÁ EXISTE ALGUM USUÁRIO COM O CPF INFORMADO
            if (usuarioCPF is not null && usuarioCPF.IdUsuario != usuario.IdUsuario)
            {
                ModelState.AddModelError(
                    "CPF",
                    "Este CPF já pertence a um usuário."
                );

                return View("Index", usuario);
            }

            _repositorio.EditarUsuario(usuario);

            return RedirectToAction("Index");
        }
    }
}