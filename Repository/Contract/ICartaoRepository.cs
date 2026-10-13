using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICartaoRepository
    {
        void AdicionarCartao(Cartao cartao);
        void EditarCartao(Cartao cartao);
        Cartao BuscarCartaoPorId(int id);
        List<Cartao> BuscarTodos();
    }
}
