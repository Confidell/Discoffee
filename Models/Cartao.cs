using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Cartao
    {
        public int IdCartao { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}\s?\d{4}\s?\d{4}\s?\d{4}$", ErrorMessage = "Número de cartão inválido. Use 16 dígitos.")]
        public string NumeroCartao { get; set; }

        [Required]
        public string NomeTitular { get; set; }

        public string Apelido { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Data de validade inválida. Use o formato MM/AA.")]
        public string DataValidade { get; set; }
    }
}
