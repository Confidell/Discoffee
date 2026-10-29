using System.Text.Json;
using Discoffee.Models;
using Discoffee.Repository.Contract;
using Discoffee.Sessao;
using Discoffee.ViewModels;
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

            // Busca o produto no banco.
            var produto = _produtoRepository.BuscarProdutoPorId(idProduto);

            if (produto == null || !produto.Disponibilidade)
            {
                return BadRequest(new
                {
                    mensagem = "Produto indisponível ou não encontrado."
                });
            }

            // Captura os IDs das opções selecionadas no modal.
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

            // Busca as opções válidas para a categoria do produto.
            var opcoesCategoria =
                _opcaoPersonalizacaoRepository
                    .BuscarOpcoesPorCategoria(produto.Categoria.IdCategoria);

            // Valida se todos os IDs enviados pertencem à categoria.
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

            // Monta o item usando o ViewModel que você já possui.
            var item = new ItemPedidoViewModel
            {
                IdProduto = produto.IdProduto,
                NomeProduto = produto.Nome,
                Imagem = produto.Imagem,
                PrecoUnitario = produto.Preco,
                Quantidade = quantidade,
                Opcoes = opcoesSelecionadas
            };

            // Salva o item na sessão usando Newtonsoft.Json.
            _sessaoItemPedido.AdicionarItem(item);

            return Ok(new
            {
                mensagem = "Produto adicionado ao carrinho!"
            });
        }

        // [HttpPost]
        // public IActionResult Atualizar(ItemPedidoViewModel item)
        // {
        //     _sessaoItemPedido.Atualizar(item);

        //     return RedirectToAction(nameof(Index));
        // }

        [HttpPost]
        public IActionResult Remover(ItemPedidoViewModel item)
        {
            _sessaoItemPedido.RemoverItem(item.IdItem);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public void FinalizarCompra([FromBody] Pedido pedido)
        {
            int idPedido = _pedidoRepository.CriarPedido(pedido);

            // PASSA POR CADA UM DOS PRODUTOS NO CARRINHO
            foreach (var item in pedido.Itens)
            {
                var itemPedido = new ItemPedidoViewModel
                {
                    IdPedido = idPedido,
                    IdProduto = item.IdProduto,
                    Quantidade = item.Quantidade
                };

                // INSERE O PRODUTO E PEGA SEU ID
                int idItem = _pedidoRepository.InserirItemPedido(
                    itemPedido
                );

                // INSERE AS PERSONALIZAÇÕES DESSE PRODUTO
                foreach (var opcao in item.Opcoes)
                {
                    var opcaoItem = new OpcaoPedidoViewModel
                    {
                        IdItem = idItem,
                        IdOpcao = opcao.IdOpcao
                    };

                    _pedidoRepository.InserirOpcaoItemPedido(opcaoItem);
                }
            }

            // RECALCULA O TOTAL
            _pedidoRepository.AtualizarTotalPedido(idPedido);
        }
    }
}
