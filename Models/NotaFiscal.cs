using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class NotaFiscal
    {
        public int IdNota { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public decimal ValorTotal { get; set; }

        [Required]
        public int Numeracao { get; set; }

        [Required]
        public string CNPJ_Discoffee { get; set; }
    }
}
