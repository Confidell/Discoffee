using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface INotaFiscal
    {
        void EmitirNota(NotaFiscal nota);
        void EditarNota(NotaFiscal nota);
        void ExcluirNota(int id);
        List<NotaFiscal> BuscarNotas();
        List<NotaFiscal> BuscarNotasPorUsuario(int usuarioId);
    }
}
