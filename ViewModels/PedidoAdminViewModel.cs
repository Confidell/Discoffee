using Discoffee.Models.Constants;

namespace Discoffee.ViewModels
{
    public class PedidoAdminViewModel
    {
        public int IdPedido { get; set; }
        public DateTime DataHora { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Frete { get; set; }
        public decimal Total { get; set; }

        public StatusPedido Status { get; set; }
        public TipoPedido Tipo { get; set; }

        public int UsuarioId { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

        public List<ItemPedidoViewModel> Itens { get; set; } = new();
    }
}