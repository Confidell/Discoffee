using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Usuario
    {
        [Display(Name = "Id do usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "Nome completo é obrigatório!")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email é obrigatório!")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Senha é obrigatória!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, uma letra maiúscula, uma minúscula, um número e um caractere especial.")]
        public string Senha { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF é obrigatório!")]
        public string CPF { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "Telefone é obrigatório!")]
        [RegularExpression(@"^(\d{2}) (\d{5}-\d{4})$", ErrorMessage = "Telefone deve estar no formato (11) 99999-9999")]
        public string Telefone { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Data de Nascimento é obrigatória!")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Complemento")]
        [Required(ErrorMessage = "Complemento é obrigatório!")]
        public string Complemento { get; set; }

        [Display(Name = "Numero da Casa")]
        [Required(ErrorMessage = "Numero da casa é obrigatório!")]
        public string NumeroCasa { get; set; }
    }
}
