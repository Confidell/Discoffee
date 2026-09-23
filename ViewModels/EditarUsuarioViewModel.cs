using Discoffee.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Discoffee.ViewModels
{
    public class EditarUsuarioViewModel
    {
        [Display(Name = "Id do usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Nome Completo")]
        public string? NomeCompleto { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Insira um email válido!")]
        public string? Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma minúscula, um número e um caractere especial.")]
        public string? Senha { get; set; }

        [Display(Name = "CPF")]
        public string? CPF { get; set; }

        [Display(Name = "Telefone")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "O telefone deve possuir no máximo 11 números.")]
        public string? Telefone { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Display(Name = "Numero da Casa")]
        public string? NumeroCasa { get; set; }

        [Display(Name = "CEP")]
        public string? CEP { get; set; }
        
        public EnumPerfil Perfil { get; set; }
    }
}
