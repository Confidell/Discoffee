using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Cartao
    {
        [Display(Name = "IdCartao", Description = "Identificador do cartão.")]
        public int IdCartao { get; set; }

        [Display(Name = "Número do Cartão", Description = "Número do cartão.")]
        [Required(ErrorMessage = "O número do cartão é obrigatório.")]
        [RegularExpression(@"^\d{4}\s?\d{4}\s?\d{4}\s?\d{4}$", ErrorMessage = "Número de cartão inválido. Use 16 dígitos.")]
        public string NumeroCartao { get; set; }

        [Display(Name = "Nome do Titular", Description = "Nome do titular do cartão.")]
        [Required(ErrorMessage = "O nome do titular é obrigatório.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s]{3,150}$", ErrorMessage = "Nome do titular deve conter apenas letras e espaços.")]
        public string NomeTitular { get; set; }
        
        [Display(Name = "Apelido", Description = "Apelido do titular do cartão.")]  
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s]{0,50}$", ErrorMessage = "Apelido contém caracteres inválidos.")]
        public string Apelido { get; set; }

        [Display(Name = "Status", Description = "Status do cartão.")]
        [Required(ErrorMessage = "O status do cartão é obrigatório.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Data de validade inválida. Use o formato MM/AA.")]
        public string DataValidade { get; set; }

        [Display(Name = "Usuário", Description = "Usuário proprietário do cartão.")]
        public Usuario Usuario { get; set; }
    }
}
