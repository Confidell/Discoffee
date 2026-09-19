using Discoffee.Models;

namespace Discoffee.Repository.Contract
{
    public interface IPerfil
    {
        void CadastrarPerfil(Perfil perfil);
        void AtualizarPerfil(Perfil perfil);
        Perfil BuscarPerfil(int id);
    }
}
