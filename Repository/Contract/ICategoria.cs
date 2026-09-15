using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICategoria
    {
        void CadastrarCategoria(Categoria categoria);
        void EditarCategoria(Categoria categoria);
        Categoria BuscarCategoria(int id);
    }
}
