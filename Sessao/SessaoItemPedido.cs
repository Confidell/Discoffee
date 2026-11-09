using Discoffee.ViewModels;
using Newtonsoft.Json;

namespace Discoffee.Sessao
{
    public class SessaoItemPedido
    {
        private const string Chave = "Pedido.Itens";
        private readonly ISession _sessao;

        public SessaoItemPedido(IHttpContextAccessor accessor)
        {
            _sessao = accessor.HttpContext!.Session;
        }

        public List<ItemPedidoViewModel> Consultar()
        {
            string? json = _sessao.GetString(Chave);

            if (string.IsNullOrEmpty(json))
                return new List<ItemPedidoViewModel>();

            return JsonConvert.DeserializeObject<List<ItemPedidoViewModel>>(json)
                   ?? new List<ItemPedidoViewModel>();
        }

        public void AdicionarItem(ItemPedidoViewModel item)
        {
            var itens = Consultar();

            // GERA UM NOVO ID PARA IDENTIFICAR NO CARRINHO
            int proximoId = itens.Count == 0
                ? 1
                : itens.Max(i => i.IdItemCarrinho ?? 0) + 1;

            item.IdItemCarrinho = proximoId;
            itens.Add(item);

            string json = JsonConvert.SerializeObject(itens);
            _sessao.SetString(Chave, json);
        }

        public void AtualizarItem(int idItemCarrinho, int quantidade)
        {
            var itens = Consultar();

            var item = itens.FirstOrDefault(
                i => i.IdItemCarrinho == idItemCarrinho
            );

            if (item == null || quantidade < 1)
                return;

            item.Quantidade = quantidade;

            string json = JsonConvert.SerializeObject(itens);
            _sessao.SetString(Chave, json);
        }

        public void RemoverItem(int idItemCarrinho)
        {
            var itens = Consultar();
            itens.RemoveAll(i => i.IdItemCarrinho == idItemCarrinho);

            string json = JsonConvert.SerializeObject(itens);
            _sessao.SetString(Chave, json);
        }

        public void Limpar()
        {
            _sessao.Remove(Chave);
        }
    }
}
