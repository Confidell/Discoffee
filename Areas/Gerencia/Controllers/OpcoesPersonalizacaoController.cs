using Discoffee.Models;
using Discoffee.Repository;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    [Authorize(Roles = "Administrador, Funcionario")]
    public class OpcoesPersonalizacaoController : Controller
    {
        private readonly IOpcaoPersonalizacaoRepository _opcaoPersonalizacaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public OpcoesPersonalizacaoController(IOpcaoPersonalizacaoRepository opcaoPersonalizacaoRepository,
            ICategoriaRepository categoriaRepository)
        {
            _opcaoPersonalizacaoRepository = opcaoPersonalizacaoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult Index()
        {
            ViewBag.Categorias = _categoriaRepository.BuscarTodas();
            var opcoes = _opcaoPersonalizacaoRepository.BuscarOpcao();
            return View(opcoes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Excluir(int idOpcao)
        {
            try
            {
                _opcaoPersonalizacaoRepository.ExcluirOpcao(idOpcao);
                TempData["Sucesso"] = "Opção excluída.";
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                TempData["Erro"] = "Essa opção já foi usada em um pedido e não pode ser excluída.";
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Cadastrar(OpcaoPersonalizacao novaOpcao, string precoExtra, string nomeCategoria)
        {
            if (_opcaoPersonalizacaoRepository.BuscarOpcao()
                .Any(o => o.Opcao.Equals(novaOpcao.Opcao, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Erro"] = "Já existe uma opção com esse nome.";
                return RedirectToAction(nameof(Index));
            }

            novaOpcao.PrecoExtra = decimal.Parse(precoExtra.Replace(',', '.'), CultureInfo.InvariantCulture);
            novaOpcao.Categoria = new Categoria { NomeCategoria = nomeCategoria };
            _opcaoPersonalizacaoRepository.AdicionarOpcao(novaOpcao);
            TempData["Sucesso"] = "Opção cadastrada.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(OpcaoPersonalizacao opcaoEditada, string precoExtra, int idCategoria)
        {
            // outra opção (id diferente) já usa esse nome?
            if (_opcaoPersonalizacaoRepository.BuscarOpcao()
                .Any(o => o.IdOpcao != opcaoEditada.IdOpcao &&
                          o.Opcao.Equals(opcaoEditada.Opcao, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Erro"] = "Já existe uma opção com esse nome.";
                return RedirectToAction(nameof(Index));
            }

            opcaoEditada.PrecoExtra = decimal.Parse(precoExtra.Replace(',', '.'), CultureInfo.InvariantCulture);
            opcaoEditada.Categoria = new Categoria { IdCategoria = idCategoria };
            _opcaoPersonalizacaoRepository.EditarOpcao(opcaoEditada);
            TempData["Sucesso"] = "Opção atualizada.";
            return RedirectToAction(nameof(Index));
        }
    }
}
