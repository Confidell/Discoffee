using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICartaoRepository
    {
        void AdicionarCartao(CartaoRepository cartao);
        void EditarCartao(CartaoRepository cartao);
        CartaoRepository BuscarCartaoPorId(int id);
        List<CartaoRepository> BuscarTodos();
    }
}
