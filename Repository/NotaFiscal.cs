using Discoffee.Repository.Contract;

namespace Discoffee.Repository
{
    public class NotaFiscal : INotaFiscal
    {
        public List<NotaFiscal> BuscarNotas()
        {
            throw new NotImplementedException();
        }

        public List<NotaFiscal> BuscarNotasPorUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public void EditarNota(NotaFiscal nota)
        {
            throw new NotImplementedException();
        }

        public void EmitirNota(NotaFiscal nota)
        {
            throw new NotImplementedException();
        }

        public void ExcluirNota(int id)
        {
            throw new NotImplementedException();
        }
    }
}
