namespace Discoffee.Repository.Contract
{
    public interface ICarrinho
    {
        void AdicionarProduto(int usuarioId, int produtoId, int quantidade);
        void EditarProdutoCarrinho(int carrinhoId, int quantidade);
        void ExcluirProdutoCarrinho(int carrinhoId);
        decimal CalcularTotal(int usuarioId);
    }
}
