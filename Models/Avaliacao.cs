using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Avaliacao
    {
        public int IdAvaliacao { get; set; }

        [Required]
        [RegularExpression(@"^[1-5]$", ErrorMessage = "Nota deve ser um número inteiro entre 1 e 5.")]
        public int Nota { get; set; }

        [Required]
        public string Comentario { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}\/\d{2}\/\d{4}$", ErrorMessage = "Data inválida. Use o formato dd/MM/aaaa.")]
        public DateTime Data { get; set; }
    }
}
