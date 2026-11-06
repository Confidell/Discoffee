using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICartaoRepository
    {
        void AdicionarCartao(Cartao cartao, int IdUsuario);
        void EditarCartao(Cartao cartao);
        Cartao BuscarCartaoPorId(int id);
        List<Cartao> BuscarTodosPorUsuario(int IdUsuario);
    }
}
