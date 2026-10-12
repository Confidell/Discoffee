using Discoffee.Models;

namespace Discoffee.ViewModels
{
    public class ProdutoModalViewModel
    {
        public Produto Produto { get; set; } = new();

        public List<OpcaoPersonalizacao> Opcoes { get; set; } = new();
    }
}