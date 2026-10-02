using Discoffee.Models;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class CardapioController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;

        public CardapioController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        {
            var produtos = _produtoRepository.BuscarTodos();
            return View(produtos);
        }
    }
}
