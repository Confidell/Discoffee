using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IUsuarioRepository
    {
        void CadastrarUsuario(Usuario usuario);
        Usuario LoginUsuario(string Email, string Senha);
        Usuario BuscarUsuarioPorId(int Id);
        List<Usuario> BuscarTodos();
        void EditarUsuario(Usuario Usuario);
        Usuario BuscarUsuarioPorEmail(string Email);
        bool BuscaUsuarioPorCPF(string CPF);
    }
}
