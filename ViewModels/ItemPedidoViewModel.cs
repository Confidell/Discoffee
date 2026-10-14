namespace Discoffee.ViewModels
{
    public class ItemPedidoViewModel
    {
        public int? IdPedido { get; set; }
        public int? IdProduto { get; set; }
        public int? Quantidade { get; set; }
        public List<int>? Opcoes { get; set;} = new();
    }
}