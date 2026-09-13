using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICupom
    {
        void AdicionarCupom(Cupom cupom);
        void EditarCupom(Cupom cupom);
        void ExcluirCupom(int id);
        Cupom BuscarCupomPorCodigo(string codigo);
    }
}
