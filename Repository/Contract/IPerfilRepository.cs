using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IPerfilRepository
    {
        void CadastrarPerfil(PerfilRepository perfil);
        void AtualizarPerfil(PerfilRepository perfil);
        PerfilRepository BuscarPerfil(int id);
    }
}
