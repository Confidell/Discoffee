using System.Globalization;
using Discoffee.Models;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IWebHostEnvironment _env;   // dá acesso ao caminho da pasta wwwroot

        private static readonly string[] ExtensoesValidas = { ".jpg", ".jpeg", ".png", ".webp" };

        public ProdutosController(IProdutoRepository produtoRepository,
                                  ICategoriaRepository categoriaRepository,
                                  IWebHostEnvironment env)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _env = env;
        }

        public IActionResult Index() => CarregarIndex();

        // CADASTRAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar([FromForm] ProdutoFormViewModel vm)
        {
            if (!ValidarFormulario(vm, imagemObrigatoria: true, out var preco))
                return CarregarIndex();

            var nomeImagem = SalvarImagem(vm.ImagemArquivo!);

            if (!_produtoRepository.AdicionarProduto(MontarProduto(vm, preco, nomeImagem)))
            {
                ApagarImagem(nomeImagem);
                ModelState.AddModelError("", "Já existe um produto com esse nome e tamanho.");
                return CarregarIndex();
            }

            return RedirectToAction(nameof(Index));
        }

        // EDITAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar([FromForm] ProdutoFormViewModel vm)
        {
            // Busca o produto no banco
            var atual = _produtoRepository.BuscarProdutoPorId(vm.IdProduto);
            if (atual == null) return NotFound();

            // Na edição a imagem é opcional
            if (!ValidarFormulario(vm, imagemObrigatoria: false, out var preco))
                return CarregarIndex();

            string? novaImagem = vm.ImagemArquivo is { Length: > 0 } ? SalvarImagem(vm.ImagemArquivo) : null;

            var produto = MontarProduto(vm, preco, novaImagem ?? atual.Imagem);
            produto.IdProduto = vm.IdProduto;

            if (!_produtoRepository.EditarProduto(produto))
            {
                ApagarImagem(novaImagem);
                ModelState.AddModelError("", "Já existe outro produto com esse nome e tamanho.");
                return CarregarIndex();
            }

            if (novaImagem != null) ApagarImagem(atual.Imagem);

            return RedirectToAction(nameof(Index));
        }

        // Validações que as anotações do ViewModel não cobrem. Cada erro vira um popup no layout.
        private bool ValidarFormulario(ProdutoFormViewModel vm, bool imagemObrigatoria, out decimal preco)
        {
            if (!decimal.TryParse((vm.Preco ?? "").Replace(',', '.'),
                                  NumberStyles.Number, CultureInfo.InvariantCulture, out preco) || preco <= 0)
                ModelState.AddModelError("", "Preço inválido.");

            if (!_categoriaRepository.BuscarTodas().Any(c => c.IdCategoria == vm.CategoriaId))
                ModelState.AddModelError("", "Categoria inválida.");

            var arquivo = vm.ImagemArquivo;
            if (arquivo == null || arquivo.Length == 0)
            {
                if (imagemObrigatoria) ModelState.AddModelError("", "A imagem é obrigatória.");
            }
            else
            {
                var ext = Path.GetExtension(arquivo.FileName).ToLower();
                if (!ExtensoesValidas.Contains(ext))
                    ModelState.AddModelError("", "A imagem deve ser .jpg, .jpeg, .png ou .webp.");
                if (arquivo.Length > 5 * 1024 * 1024)
                    ModelState.AddModelError("", "A imagem deve ter no máximo 5 MB.");
            }

            return ModelState.IsValid;
        }

        // Converte o ViewModel (formulário) em Produto (banco)
        private static Produto MontarProduto(ProdutoFormViewModel vm, decimal preco, string imagem) => new()
        {
            Nome = vm.Nome.Trim(),
            Descricao = vm.Descricao?.Trim() ?? "",
            Preco = preco,
            Imagem = imagem,
            Disponibilidade = vm.Disponibilidade,
            // CategoriaId = vm.CategoriaId
        };

        // Grava a imagem em wwwroot/Assets/Imagens com nome único (GUID) e devolve o nome
        private string SalvarImagem(IFormFile arquivo)
        {
            var nome = Guid.NewGuid().ToString("N") + Path.GetExtension(arquivo.FileName).ToLower();
            var pasta = Path.Combine(_env.WebRootPath, "Assets", "Imagens");
            Directory.CreateDirectory(pasta);

            using var stream = new FileStream(Path.Combine(pasta, nome), FileMode.Create);
            arquivo.CopyTo(stream);
            return nome;
        }

        // Apaga uma imagem da pasta, se existir. GetFileName evita caminhos maliciosos
        private void ApagarImagem(string? nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return;
            var caminho = Path.Combine(_env.WebRootPath, "Assets", "Imagens", Path.GetFileName(nome));
            if (System.IO.File.Exists(caminho)) System.IO.File.Delete(caminho);
        }

        // Monta a Index (usada no Index e quando há erro de validação)
        private IActionResult CarregarIndex()
        {
            ViewBag.Categorias = _categoriaRepository.BuscarTodas();
            return View("Index", _produtoRepository.BuscarTodos());
        }
    }
}