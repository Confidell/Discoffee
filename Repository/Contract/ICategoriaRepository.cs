using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface ICategoriaRepository
    {
        List<Categoria> BuscarTodas();
        void CadastrarCategoria(CategoriaRepository categoria);
        void EditarCategoria(CategoriaRepository categoria);
        CategoriaRepository BuscarCategoria(int id);
    }
}
