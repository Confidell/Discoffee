using System.Security.Claims;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        private bool EmailFuncionario(string email)
        {
            return email.EndsWith(
                "@discoffee.com",
                StringComparison.OrdinalIgnoreCase
            );
        }

        private bool EmailAdministrador(string email)
        {
            return email.EndsWith(
                "@discoffee.gerencia.com",
                StringComparison.OrdinalIgnoreCase
            );
        }

        public async Task<IActionResult> LoginGerencia(LoginGerencia loginGerencia)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Conta");
            }

            var emailFuncionario = EmailFuncionario(loginGerencia.EmailInstitucional);
            var emailAdministrador = EmailAdministrador(loginGerencia.EmailInstitucional);

            if (!emailFuncionario && !emailAdministrador)
            {
                ModelState.AddModelError(
                    "Login",
                    "Utilize um e-mail institucional Discoffee válido."
                );

                ViewData["Formulario"] = "login";
                return RedirectToAction("Index", "Conta");
            }

            Usuario? funcionarioExistente = _usuarioRepository.BuscarUsuarioPorEmail(loginGerencia.EmailInstitucional);
            
            if (funcionarioExistente is null)
            {
                ModelState.AddModelError(
                    "Login",
                    "Nenhum usuário encontrado para este email."
                );

                ViewData["Formulario"] = "login";

                return RedirectToAction("Index", "Conta");
            }

            var senhaVerificada = _passwordHasher.VerifyHashedPassword(
                funcionarioExistente,
                funcionarioExistente.Senha,
                loginGerencia.Senha
            );

            if (senhaVerificada == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    "Login",
                    "E-mail ou senha inválidos."
                );

                ViewData["Formulario"] = "login";

                return RedirectToAction("Index", "Conta");
            }

            if (emailFuncionario && funcionarioExistente.Perfil != EnumPerfil.Funcionario)
            {
                ModelState.AddModelError(
                    "Login",
                    "Este e-mail não corresponde ao perfil cadastrado."
                );

                ViewData["Formulario"] = "login";
                return RedirectToAction("Index", "Conta");
            }

            if (emailAdministrador && funcionarioExistente.Perfil != EnumPerfil.Administrador)
            {
                ModelState.AddModelError(
                    "Login",
                    "Este e-mail não corresponde ao perfil cadastrado."
                );

                ViewData["Formulario"] = "login";
                return RedirectToAction("Index", "Conta");
            }

            // CRIANDO COOKIES PARA SALVAR SESSÃO DO USUARIO
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, funcionarioExistente.IdUsuario.ToString()), // SALVA O ID
                new Claim(ClaimTypes.Name, funcionarioExistente.NomeCompleto), // SALVA O NOME
                new Claim(ClaimTypes.Email, funcionarioExistente.Email), // SALVA O EMAIL
                new Claim(ClaimTypes.Role, funcionarioExistente.Perfil.ToString()) // SALVA O PERFIL PARA LIMITAR AS AÇÕES
            };

            var identidade = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identidade);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Dashboard", "Index");
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
