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

            itens.Add(item);

            string json = JsonConvert.SerializeObject(itens);

            _sessao.SetString(Chave, json);
        }

        public void RemoverItem(int idItem)
        {
            var itens = Consultar();

            itens.RemoveAll(i => i.IdItem == idItem);

            string json = JsonConvert.SerializeObject(itens);

            _sessao.SetString(Chave, json);
        }

        public void Limpar()
        {
            _sessao.Remove(Chave);
        }
    }
}
