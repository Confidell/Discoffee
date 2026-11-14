using Discoffee.Models;
using Discoffee.Models.Constants;

namespace Discoffee.Repository.Contract
{
    public interface IPagamentoRepository
    {
        void EfetuarPagamento(Pagamento pagamento);
        void AlterarStatusPagamento(int idPagamento, EnumStatusPagamento statusPagamento);
        List<Pagamento> VerPagamentosPorUsuario(int usuarioId);
        List<Pagamento> VerTodos();
    }
}
