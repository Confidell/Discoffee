using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IProdutoRepository
    {
        bool AdicionarProduto(Produto produto);  
        bool EditarProduto(Produto produto);
        Produto BuscarProdutoPorId(int id);
        List<Produto> BuscarTodos();
    }
}
