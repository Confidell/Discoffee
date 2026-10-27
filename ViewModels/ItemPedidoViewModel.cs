using Discoffee.Models;

namespace Discoffee.ViewModels
{
    public class ItemPedidoViewModel
    {
        public int IdItem { get; set; }
        public int? IdPedido { get; set; }
        public int? IdProduto { get; set; }
        public string? NomeProduto { get; set; }
        public string? Imagem { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public int? Quantidade { get; set; }
        public List<OpcaoPersonalizacao>? Opcoes { get; set;} = new();
    }
}