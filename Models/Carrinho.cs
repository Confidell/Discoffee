using System.ComponentModel.DataAnnotations;
using Discoffee.ViewModels;

namespace Discoffee.Models
{
    public class Carrinho
    {
        public List<ItemPedidoViewModel> Itens { get; set; } = new();
    }
}