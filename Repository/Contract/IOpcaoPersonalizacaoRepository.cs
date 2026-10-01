using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IOpcaoPersonalizacaoRepository
    {
        void EditarOpcao(OpcaoPersonalizacao opcao);
        void ExcluirOpcao(int id);
        void AdicionarOpcao(OpcaoPersonalizacao opcao);
        OpcaoPersonalizacao BuscarOpcaoPorId(int id);
        List<OpcaoPersonalizacao> BuscarOpcao();
    }
}
