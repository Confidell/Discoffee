using Discoffee.Repository.Contract;

namespace Discoffee.Repository
{
    public class Usuario : IUsuario
    {
        public List<Usuario> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarUsuarioPorId(int id)
        {
            throw new NotImplementedException();
        }

        public void CadastrarUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void EditarUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Usuario LoginUsuario(string email, string senha)
        {
            throw new NotImplementedException();
        }
    }
}
