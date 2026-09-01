using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Pagamento
    {
        public int IdPagamento { get; set; }

        public FormaPagamento FormaPagamento { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        [Required]
        public decimal ValorPago { get; set; }

        [Required]
        public string Status { get; set; }

    }
}
