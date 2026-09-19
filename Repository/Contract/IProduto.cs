using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IProduto
    {
        void EditarProduto(Produto produto);
        void ExcluirProduto(int id);
        void AdicionarProduto(Produto produto);
        Produto BuscarProdutoPorId(int id);
        List<Produto> BuscarTodos();
    }
}
