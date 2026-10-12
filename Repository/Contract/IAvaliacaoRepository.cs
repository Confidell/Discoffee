using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IAvaliacaoRepository
    {
        void AdicionarAvaliacao(AvaliacaoRepository avaliacao);
        void EditarAvaliacao(AvaliacaoRepository avaliacao);
        void ExcluirAvaliacao(int id);
        List<AvaliacaoRepository> BuscarAvaliacoes();
        List<AvaliacaoRepository> BuscarAvaliacoesPorProduto(int produtoId);
        List<AvaliacaoRepository> BuscarAvaliacoesPorUsuario(int usuarioId);
    }
}
