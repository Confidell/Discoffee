using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IAvaliacao
    {
        void AdicionarAvaliacao(Avaliacao avaliacao);
        void EditarAvaliacao(Avaliacao avaliacao);
        void ExcluirAvaliacao(int id);
        List<Avaliacao> BuscarAvaliacoes();
        List<Avaliacao> BuscarAvaliacoesPorProduto(int produtoId);
        List<Avaliacao> BuscarAvaliacoesPorUsuario(int usuarioId);
    }
}
