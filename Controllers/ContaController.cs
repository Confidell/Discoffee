using System.Globalization;
using System.Security.Claims;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class ContaController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();
        public ContaController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        
        [HttpGet]
        public IActionResult Index(string? form, string? returnUrl)
        {
            ViewData["Formulario"] = form;
            ViewBag.ReturnUrl = returnUrl;

            return View(new ContaViewModel());
        }

        // [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(CadastrarUsuarioViewModel usuarioViewModel, string? form)
        {

            Usuario usuarioExistente = _usuarioRepository.BuscarUsuarioPorEmail(usuarioViewModel.Email);

            // VALIDA A DATA DE NASCIMENTO PARA O FORMATO CORRETO
            if (!DateTime.TryParseExact(
                usuarioViewModel.DataNascimento,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dataNascimento))
            {
                ModelState.AddModelError(
                    "DataNascimento",
                    "Informe uma data de nascimento válida."
                );
            }

            // VERIFICA SE JÁ EXISTE ALGUM USUÁRIO CADASTRADO
            if (usuarioExistente is not null)
            {
                ModelState.AddModelError(
                    "Email",
                    "Este e-mail já está cadastrado."
                );
            }

            // VERIFICA SE JÁ EXISTE ALGUM USUÁRIO COM O CPF INFORMADO
            if (_usuarioRepository.BuscaUsuarioPorCPF(usuarioViewModel.CPF) is not null)
            {
                ModelState.AddModelError(
                    "CPF",
                    "Este CPF já pertence a um usuário."
                );
            }

            // VERIFICA CONFIRMAÇÃO DE EMAIL
            if (usuarioViewModel.Email != usuarioViewModel.ConfirmarEmail)
            {
                ModelState.AddModelError(
                    "ConfirmarEmail",
                    "Os e-mails não são iguais."
                );
            }

            // VERIFICA CONFIRMAÇÃO DE SENHA
            if (usuarioViewModel.Senha != usuarioViewModel.ConfirmarSenha)
            {
                ModelState.AddModelError(
                    "ConfirmarSenha",
                    "As senhas não são iguais."
                );
            }

            // VERIFICA SE ACEITOU OS TERMOS
            if (!usuarioViewModel.AceitouTermos)
            {
                ModelState.AddModelError(
                    "AceitouTermos",
                    "Você precisa aceitar os termos de uso."
                );
            }

            // RETORNA AO FORM CASO HAJA ERRO
            if (!ModelState.IsValid)
            {
                var mensagens = ModelState
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                TempData["Erros"] = string.Join("|", mensagens);

                ViewData["Formulario"] = form;

                return View("Index", new ContaViewModel
                {
                    Cadastrar = usuarioViewModel
                });
            }

            // CRIA O OBJETO DO NOVO USUÁRIO
            Usuario usuario = new Usuario
            {
                NomeCompleto = usuarioViewModel.NomeCompleto,
                Email = usuarioViewModel.Email,
                Senha = usuarioViewModel.Senha,
                CPF = usuarioViewModel.CPF,
                Telefone = usuarioViewModel.Telefone,
                DataNascimento = dataNascimento,
                CEP = usuarioViewModel.CEP,
                NumeroCasa = usuarioViewModel.NumeroCasa,
                Complemento = usuarioViewModel.Complemento,
                Perfil = EnumPerfil.Cliente
            };

            // CRIPTOGRAFA A SENHA
            usuario.Senha = _passwordHasher.HashPassword(
                usuario,
                usuario.Senha
            );

            _usuarioRepository.CadastrarUsuario(usuario);

            return RedirectToAction("Index", "Conta", new { form = "login" });

        }

        [HttpPost]
        public async Task<IActionResult> Login(
            LoginViewModel loginViewModel,
            string? form,
            string? returnUrl)
        {
            Usuario? usuario = _usuarioRepository.BuscarUsuarioPorEmail(loginViewModel.Email);

            // USUÁRIO NÃO ENCONTRADO
            if (usuario is null)
            {
                ModelState.AddModelError(
                    "Login",
                    "E-mail ou senha inválidos."
                );

                ViewData["Formulario"] = "login";
                return View("Index", new ContaViewModel
                {
                    Login = loginViewModel
                });
            }

            // COMPARA SENHA ENVIADA COM A SENHA CRIPTOGRAFADA NO BANCO
            var senhaVerificada = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                loginViewModel.Senha
            );

            if (senhaVerificada == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    "Login",
                    "E-mail ou senha inválidos."
                );

                ViewData["Formulario"] = "login";
                return View("Index", new ContaViewModel
                {
                    Login = loginViewModel
                });
            }

            // CRIANDO COOKIES PARA SALVAR SESSÃO DO USUÁRIO
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.NomeCompleto),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
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

            // REDIRECIONAMENTO DE ACORDO COM O PERFIL
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                if (returnUrl.Contains("/Cliente/Pedido/FinalizarCompra"))
                {
                    return RedirectToAction("Index", "Carrinho");
                }

                return Redirect(returnUrl);
            }

            switch (usuario.Perfil)
            {
                case EnumPerfil.Cliente:
                    return RedirectToAction("Index", "Home");

                case EnumPerfil.Funcionario:
                case EnumPerfil.Administrador:
                    return RedirectToAction("Index", "Dashboard", new { area = "Gerencia" });

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Index", "Home");
        }
    }
}
