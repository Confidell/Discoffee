using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IProdutoRepository
    {
        void AdicionarProduto(Produto produto);  
        void EditarProduto(Produto produto);
        Produto BuscarProdutoPorId(int id);
        List<Produto> BuscarTodos();
    }
}
