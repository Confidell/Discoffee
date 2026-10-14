namespace Discoffee.Models
{
    public class ItemPedido
    {
        public int IdItem { get; set; }

        public int PedidoId { get; set; }

        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }
        
        public decimal PrecoUnitario { get; set; }

        public string? NomeProduto { get; set; }

        public string? Tamanho { get; set; }

        // Personalizações deste item
        public List<OpcaoPersonalizacao> Opcoes { get; set; } = new();
    }
}