using System.Globalization;
using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class ContaController : Controller
    {
        private readonly IUsuarioRepository _repositorio;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();
        public ContaController(IUsuarioRepository repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(CadastrarUsuarioViewModel usuarioViewModel, string form)
        {
            Usuario usuarioExistente = _repositorio.BuscarUsuarioPorEmail(usuarioViewModel.Email);

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
            if (_repositorio.BuscaUsuarioPorCPF(usuarioViewModel.CPF))
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

                return View("Index", usuarioViewModel);
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

            _repositorio.CadastrarUsuario(usuario);

            return RedirectToAction("Index", "Home");
        }
    }
}
