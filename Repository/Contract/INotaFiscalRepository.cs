using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface INotaFiscalRepository
    {
        void EmitirNota(NotaFiscalRepository nota);
        void EditarNota(NotaFiscalRepository nota);
        void ExcluirNota(int id);
        List<NotaFiscalRepository> BuscarNotas();
        List<NotaFiscalRepository> BuscarNotasPorUsuario(int usuarioId);
    }
}
