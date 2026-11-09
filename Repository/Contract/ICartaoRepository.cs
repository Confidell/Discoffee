using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICartaoRepository
    {
        void AdicionarCartao(Cartao cartao, int IdUsuario);
        Cartao BuscarCartaoPorId(int IdCartao);
        void DeletarCartao(int IdCartao);
        List<Cartao> BuscarTodosPorUsuario(int IdUsuario);
    }
}
