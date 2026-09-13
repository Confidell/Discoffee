using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IOpcaoPersonalizacao
    {
        void EditarOpcao(OpcaoPersonalizacao opcao);
        void ExcluirOpcao(int id);
        void AdicionarOpcao(OpcaoPersonalizacao opcao);
        OpcaoPersonalizacao BuscarOpcaoPorId(int id);
        List<OpcaoPersonalizacao> BuscarOpcao();
    }
}
