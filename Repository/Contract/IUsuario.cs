using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IUsuario
    {
        void CadastrarUsuario(Usuario usuario);
        Usuario LoginUsuario(string email, string senha);
        Usuario BuscarUsuarioPorId(int id);
        List<Usuario> BuscarTodos();
        void EditarUsuario(Usuario usuario);
    }
}
