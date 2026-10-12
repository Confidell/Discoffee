using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICupomRepository
    {
        void AdicionarCupom(CupomRepository cupom);
        void EditarCupom(CupomRepository cupom);
        void ExcluirCupom(int id);
        CupomRepository BuscarCupomPorCodigo(string codigo);
    }
}
