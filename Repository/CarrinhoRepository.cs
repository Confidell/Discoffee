using Discoffee.Repository.Contract;

namespace Discoffee.Repository
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        public void AdicionarProduto(int usuarioId, int produtoId, int quantidade)
        {
            throw new NotImplementedException();
        }

        public decimal CalcularTotal(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public void EditarProdutoCarrinho(int carrinhoId, int quantidade)
        {
            throw new NotImplementedException();
        }

        public void ExcluirProdutoCarrinho(int carrinhoId)
        {
            throw new NotImplementedException();
        }
    }
}
