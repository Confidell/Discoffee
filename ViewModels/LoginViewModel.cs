using System.ComponentModel.DataAnnotations;

namespace Discoffee.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Informe seu email!")]
        [EmailAddress(ErrorMessage = "Insira um email válido!")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Informe sua senha!")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}