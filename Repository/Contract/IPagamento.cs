using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IPagamento
    {
        void EfetuarPagamento(Pagamento pagamento);
        List<Pagamento> VerPagamentosPorUsuario(int usuarioId);
        List<Pagamento> VerTodos();
        void EditarPagamento(Pagamento pagamento);
        void ExcluirPagamento(int id);
    }
}
