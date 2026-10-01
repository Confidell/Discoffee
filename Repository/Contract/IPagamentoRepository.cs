using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IPagamentoRepository
    {
        void EfetuarPagamento(PagamentoRepository pagamento);
        List<PagamentoRepository> VerPagamentosPorUsuario(int usuarioId);
        List<PagamentoRepository> VerTodos();
        void EditarPagamento(PagamentoRepository pagamento);
        void ExcluirPagamento(int id);
    }
}
