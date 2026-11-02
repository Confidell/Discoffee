namespace Discoffee.Models
{
    public class ItemPedido
    {
        public int IdItem { get; set; }

        public int IdPedido { get; set; }

        public int IdProduto { get; set; }

        public int Quantidade { get; set; }
        
        public decimal PrecoUnitario { get; set; }
        
        public List<OpcaoPersonalizacao> Opcoes { get; set; } = new();
    }
}