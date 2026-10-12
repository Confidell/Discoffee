using Discoffee.Repository;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class CardapioController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IOpcaoPersonalizacaoRepository _opcaoPersonalizacaoRepository;
        public CardapioController(IProdutoRepository produtoRepository, IOpcaoPersonalizacaoRepository opcaoPersonalizacaoRepository)
        {
            _produtoRepository = produtoRepository;
            _opcaoPersonalizacaoRepository = opcaoPersonalizacaoRepository;
        }

        public IActionResult Index()
        {
            var produtos = _produtoRepository.BuscarTodos();
            return View(produtos);
        }



        [HttpGet]
        public IActionResult BuscarOpcoesProduto(int idCategoria)
        {
            var opcoes = _opcaoPersonalizacaoRepository
                .BuscarOpcoesPorCategoria(idCategoria);

            return PartialView("_OpcoesPersonalizacao", opcoes);
        }
    }
}
