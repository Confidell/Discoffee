using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class NotaFiscal
    {
        [Display(Name = "IdNota", Description = "Código da nota fiscal.")]
        public int IdNota { get; set; }

        [Display(Name = "DataEmissao", Description = "Data de emissão da nota fiscal.")]
        [Required(ErrorMessage = "A data de emissão é obrigatória.")]
        [RegularExpression(@"^\d{2}\/\d{2}\/\d{4}$", ErrorMessage = "Data de emissão inválida. Use o formato dd/MM/aaaa.")]
        public DateTime DataEmissao { get; set; }
        
        [Display(Name = "ValorTotal", Description = "Valor total da nota fiscal.")]
        [Required(ErrorMessage = "O valor total é obrigatório.")]
        public decimal ValorTotal { get; set; }

        [Display(Name = "Numeracao", Description = "Numeração do cupom.")]
        [Required(ErrorMessage = "A numeração é obrigatória.")]
        public int Numeracao { get; set; }

        [Display(Name = "CNPJ_Discoffee", Description = "CNPJ da Discoffee.")]
        [Required(ErrorMessage = "O CNPJ da Discoffee é obrigatório.")]
        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido. Use o formato 00.000.000/0000-00.")]
        public string CNPJ_Discoffee { get; set; }

        [Display(Name = "Cliente", Description = "Cliente associado à nota fiscal.")]
        public Usuario Cliente { get; set; }

        [Display(Name = "Pagamento", Description = "Pagamento associado à nota fiscal.")]
        public Pagamento Pagamento { get; set; }
    }
}
