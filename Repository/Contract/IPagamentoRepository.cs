using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IPagamentoRepository
    {
        void EfetuarPagamento(Pagamento pagamento);
        List<Pagamento> VerPagamentosPorUsuario(int usuarioId);
        List<Pagamento> VerTodos();
    }
}
