using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IProdutoRepository
    {
        void EditarProduto(ProdutoRepository produto);
        void ExcluirProduto(int id);
        void AdicionarProduto(ProdutoRepository produto);
        ProdutoRepository BuscarProdutoPorId(int id);
        List<ProdutoRepository> BuscarTodos();
    }
}
