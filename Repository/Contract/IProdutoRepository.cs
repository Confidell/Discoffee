using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IProdutoRepository
    {
        bool AdicionarProduto(Produto produto);  
        bool EditarProduto(Produto produto);
        void AlterarDisponibilidade(int id, bool disponivel);
        Produto BuscarProdutoPorId(int id);
        List<Produto> BuscarTodos();
    }
}
