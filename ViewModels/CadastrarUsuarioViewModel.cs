using System.ComponentModel.DataAnnotations;

namespace Discoffee.ViewModels
{
    public class CadastrarUsuarioViewModel
    {
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "Nome completo é obrigatório!")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email é obrigatório!")]
        [EmailAddress(ErrorMessage = "Insira um email válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Confirmação do e-mail é obrigatória!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string ConfirmarEmail { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Senha é obrigatória!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, maiúscula, minúscula, número e caractere especial.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmação da senha é obrigatória!")]
        public string ConfirmarSenha { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF é obrigatório!")]
        public string CPF { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "Telefone é obrigatório!")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "O telefone deve possuir no máximo 11 números.")]
        public string Telefone { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "Digite uma data válida no formato DD/MM/AAAA.")]
        public string DataNascimento { get; set; }

        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Display(Name = "Numero da Casa")]
        [Required(ErrorMessage = "Numero da casa é obrigatório!")]
        public string NumeroCasa { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "CEP é obrigatório!")]
        public string CEP { get; set; }

        public bool AceitouTermos { get; set; }
    }
}