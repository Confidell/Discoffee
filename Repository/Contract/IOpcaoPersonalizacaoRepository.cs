using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IOpcaoPersonalizacaoRepository
    {
        void EditarOpcao(OpcaoPersonalizacao opcao);
        void ExcluirOpcao(int id);
        void AdicionarOpcao(OpcaoPersonalizacao opcao);
        void InserirOpcaoPedido(OpcaoPersonalizacao opcao, int idPedido);
        OpcaoPersonalizacao BuscarOpcaoPorId(int id);
        List<OpcaoPersonalizacao> BuscarOpcoesPorCategoria(int idCategoria);
        List<OpcaoPersonalizacao> BuscarOpcao();
    }
}
