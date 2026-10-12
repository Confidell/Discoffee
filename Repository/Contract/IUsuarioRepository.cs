using Discoffee.Models;
using Discoffee.ViewModels;

namespace Discoffee.Repository.Contract
{
    public interface IUsuarioRepository
    {
        void CadastrarUsuario(Usuario usuario);
        Usuario BuscarUsuarioPorId(int Id);
        List<Usuario> BuscarTodos();
        void EditarUsuario(EditarUsuarioViewModel Usuario);
        Usuario BuscarUsuarioPorEmail(string Email);
        Usuario? BuscaUsuarioPorCPF(string CPF);
    }
}
