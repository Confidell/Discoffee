using System.Security.Claims;
using System.Text.Json;
using Discoffee.Models;
using Discoffee.Repository.Contract;
using Discoffee.Sessao;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly SessaoItemPedido _sessaoItemPedido;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IOpcaoPersonalizacaoRepository _opcaoPersonalizacaoRepository;

        public CarrinhoController(
            IPedidoRepository pedidoRepository, 
            SessaoItemPedido sessaoItemPedido,
            IProdutoRepository produtoRepository,
            IOpcaoPersonalizacaoRepository opcaoPersonalizacaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _sessaoItemPedido = sessaoItemPedido;
            _opcaoPersonalizacaoRepository = opcaoPersonalizacaoRepository;
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        {
            var itens = _sessaoItemPedido.Consultar();

            return View(itens);
        }
        
        [HttpPost]
        public IActionResult AdicionarItem([FromBody] JsonElement dados)
        {
            int idProduto = dados.GetProperty("idProduto").GetInt32();
            int quantidade = dados.GetProperty("quantidade").GetInt32();

            if (quantidade < 1)
            {
                return BadRequest(new
                {
                    mensagem = "A quantidade deve ser maior que zero."
                });
            }

            // BUSCA O PRODUTO NO BANCO
            var produto = _produtoRepository.BuscarProdutoPorId(idProduto);

            if (produto == null || !produto.Disponibilidade)
            {
                return BadRequest(new
                {
                    mensagem = "Produto indisponível ou não encontrado."
                });
            }

            // PEGA OS IDS DAS OPÇÕES SELECIONADAS
            var idsOpcoes = dados.GetProperty("opcoes")
                .EnumerateArray()
                .Select(opcao => opcao.GetProperty("idOpcao").GetInt32())
                .ToList();

            if (idsOpcoes.Distinct().Count() != idsOpcoes.Count)
            {
                return BadRequest(new
                {
                    mensagem = "Existem opções duplicadas."
                });
            }

            // BUSCA AS OPÇÕES DISPONIVEIS PARA AQUELE PRODUTO
            var opcoesCategoria =
                _opcaoPersonalizacaoRepository
                    .BuscarOpcoesPorCategoria(produto.Categoria.IdCategoria);

            // VERIFICA SE AS OPÇÕES EXISTEM NAQUELA CATEGORIA
            var opcoesSelecionadas = opcoesCategoria
                .Where(opcao => idsOpcoes.Contains(opcao.IdOpcao))
                .ToList();

            var idsEncontrados = opcoesSelecionadas
                .Select(opcao => opcao.IdOpcao)
                .ToHashSet();

            var idsInvalidos = idsOpcoes
                .Where(id => !idsEncontrados.Contains(id))
                .ToList();

            if (idsInvalidos.Any())
            {
                return BadRequest(new
                {
                    mensagem = $"Opções inválidas para este produto: {string.Join(", ", idsInvalidos)}."
                });
            }

            // MONTA O ITEM
            var item = new ItemPedidoViewModel
            {
                IdProduto = produto.IdProduto,
                NomeProduto = produto.Nome,
                Imagem = produto.Imagem,
                PrecoUnitario = produto.Preco,
                Quantidade = quantidade,
                Opcoes = opcoesSelecionadas
            };

            // SALVA O ITEM NA SESSAO
            _sessaoItemPedido.AdicionarItem(item);

            return Ok(new
            {
                mensagem = "Produto adicionado ao carrinho!"
            });
        }

        [HttpPost]
        public IActionResult AtualizarItem(int idItemCarrinho, int quantidade)
        {

            _sessaoItemPedido.AtualizarItem(idItemCarrinho, quantidade);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RemoverItem(int idItemCarrinho)
        {
            _sessaoItemPedido.RemoverItem(idItemCarrinho);

            return RedirectToAction(nameof(Index));
        }
    }
}
