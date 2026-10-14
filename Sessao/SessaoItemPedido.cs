using Discoffee.ViewModels;
using System.Text.Json;

namespace Discoffee.Sessao
{
    public class SessaoItemPedido
    {
        // PERMITE ACESSAR O HTTP ATUAL, E POR MEIO DELE, A SESSION 
        private readonly IHttpContextAccessor _httpContextAccessor;

        // A CONSTANTE CHAVE SERVE PARA IDENTIFICAR ONDE ESTA A LISTA ARMEZANADA NA SESSION
        private const string Chave = "Pedido.Itens";

       
        public SessaoItemPedido(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        // FACILITA O ACESSO À SESSION
        private ISession Sessao =>
            _httpContextAccessor.HttpContext!.Session;
        
        public void Cadastrar(ItemPedidoViewModel item)
        {
            // RECUPERA OS ITENS QUE JA ESTÃO NO CARRINHO
            var itens = Consultar();

            // ADICIONA O NOVO PRODUTO A LISTA
            itens.Add(item);

            // GUARDA O JSON NA SESSION
            Sessao.SetString(Chave, JsonSerializer.Serialize(itens));
        }

        public List<ItemPedidoViewModel> Consultar()
        {
            // PROCURA O DADOS DA SESSION
            var dados = Sessao.GetString(Chave);


            return dados == null
                ? new List<ItemPedidoViewModel>()
                : JsonSerializer.Deserialize<List<ItemPedidoViewModel>>(dados)
                    ?? new List<ItemPedidoViewModel>();
        }

        public void Atualizar(ItemPedidoViewModel item)
        {
            var itens = Consultar();

            var indice = itens.FindIndex(i =>
                i.IdPedido == item.IdPedido &&
                i.IdProduto == item.IdProduto
            );

            if (indice != -1)
            {
                itens[indice] = item;

                Sessao.SetString(
                    Chave,
                    JsonSerializer.Serialize(itens)
                );
            }
        }

        public void Remover(ItemPedidoViewModel item)
        {
            var itens = Consultar();
            itens.RemoveAll(i => i.IdPedido == item.IdPedido && i.IdProduto == item.IdProduto);

            Sessao.SetString(Chave, JsonSerializer.Serialize(itens));
        }
    }
}
