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

        // EXIBE O CARDÁPIO SOMENTE COM OS PRODUTOS DISPONÍVEIS.
        public IActionResult Index()
        {
            var produtos = _produtoRepository.BuscarTodos()
                .Where(p => p.Disponibilidade)
                .ToList();
            return View(produtos);
        }


        // BUSCA AS OPÇÕES DE PERSONALIZAÇÃO DE UMA CATEGORIA E RETORNA A VIEW PARCIAL.
        [HttpGet]
        public IActionResult BuscarOpcoesProduto(int idCategoria)
        {
            var opcoes = _opcaoPersonalizacaoRepository.BuscarOpcoesPorCategoria(idCategoria);

            return PartialView("_OpcoesPersonalizacao", opcoes);
        }
    }
}
