using Discoffee.Repository.Contract;

namespace Discoffee.Repository
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        public List<NotaFiscalRepository> BuscarNotas()
        {
            throw new NotImplementedException();
        }

        public List<NotaFiscalRepository> BuscarNotasPorUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public void EditarNota(NotaFiscalRepository nota)
        {
            throw new NotImplementedException();
        }

        public void EmitirNota(NotaFiscalRepository nota)
        {
            throw new NotImplementedException();
        }

        public void ExcluirNota(int id)
        {
            throw new NotImplementedException();
        }
    }
}
