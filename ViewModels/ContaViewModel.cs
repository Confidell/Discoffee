namespace Discoffee.ViewModels
{
    public class ContaViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();

        public CadastrarUsuarioViewModel Cadastrar { get; set; } = new CadastrarUsuarioViewModel();
    }
}