
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuariosController(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            var usuarios = _usuarioRepository.BuscarTodos();
            return View(usuarios);
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            usuario.Status = "Ativo";

            if (usuario.Perfil != EnumPerfil.Funcionario &&
                usuario.Perfil != EnumPerfil.Administrador)
            {
                ModelState.AddModelError(nameof(Usuario.Perfil), "Perfil inválido.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.Email) &&
                _usuarioRepository.BuscarUsuarioPorEmail(usuario.Email) != null)
            {
                ModelState.AddModelError(nameof(Usuario.Email), "Este e-mail já está cadastrado.");
            }

            if (!string.IsNullOrWhiteSpace(usuario.CPF) &&
                _usuarioRepository.BuscaUsuarioPorCPF(usuario.CPF) != null)
            {
                ModelState.AddModelError(nameof(Usuario.CPF), "Este CPF já está cadastrado.");
            }

            // DEVOLVE A MESMA TELA COM OS AVISOS EMBAIXO DE CADA CAMPO
            if (!ModelState.IsValid)
            {
                ViewData["UsuarioCadastro"] = usuario;
                ViewData["AbrirModal"] = true;
                return View("Index", _usuarioRepository.BuscarTodos());
            }

            // CRIPTOGRAFA A SENHA
            usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);

            _usuarioRepository.CadastrarUsuario(usuario);

            TempData["Sucesso"] = "Usuário cadastrado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Editar(EditarUsuarioViewModel form)
        {
            // O PERFIL É CONFERIDO NO BANCO, NÃO NO FORMULÁRIO
            var atual = _usuarioRepository.BuscarUsuarioPorId(form.IdUsuario);

            if (atual == null)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // CLIENTE: SÓ O STATUS PODE SER ALTERADO
            if (atual.Perfil == EnumPerfil.Cliente)
            {
                form = new EditarUsuarioViewModel
                {
                    IdUsuario = form.IdUsuario,
                    Status = form.Status
                };
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    TempData["Erro"] = "Confira os dados informados.";
                    return RedirectToAction(nameof(Index));
                }

                if (form.Perfil != EnumPerfil.Funcionario &&
                    form.Perfil != EnumPerfil.Administrador)
                {
                    TempData["Erro"] = "Perfil inválido.";
                    return RedirectToAction(nameof(Index));
                }

                // E-MAIL E CPF NÃO PODEM PERTENCER A OUTRO USUÁRIO
                var comEmail = string.IsNullOrWhiteSpace(form.Email) ? null : _usuarioRepository.BuscarUsuarioPorEmail(form.Email);
                if (comEmail != null && comEmail.IdUsuario != form.IdUsuario)
                {
                    TempData["Erro"] = "Este e-mail já está cadastrado.";
                    return RedirectToAction(nameof(Index));
                }

                form.CPF = SomenteNumeros(form.CPF);
                form.Telefone = SomenteNumeros(form.Telefone);
                form.CEP = SomenteNumeros(form.CEP);

                var comCpf = string.IsNullOrWhiteSpace(form.CPF) ? null : _usuarioRepository.BuscaUsuarioPorCPF(form.CPF);
                if (comCpf != null && comCpf.IdUsuario != form.IdUsuario)
                {
                    TempData["Erro"] = "Este CPF já está cadastrado.";
                    return RedirectToAction(nameof(Index));
                }

                // SENHA VAZIA MANTÉM A ANTIGA; PREENCHIDA É CRIPTOGRAFADA
                form.Senha = string.IsNullOrWhiteSpace(form.Senha)
                    ? null
                    : _passwordHasher.HashPassword(atual, form.Senha);
            }

            if (form.Status != "Ativo" && form.Status != "Inativo")
            {
                form.Status = null;
            }

            _usuarioRepository.EditarUsuarioGerencia(form);

            TempData["Sucesso"] = "Usuário atualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        private static string? SomenteNumeros(string? valor)
        {
            return valor == null ? null : new string(valor.Where(char.IsDigit).ToArray());
        }
    }
}
