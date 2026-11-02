using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Cupom
    {
        [Display(Name = "IdCupom", Description = "Código do cupom.")]
        public int IdCupom { get; set; }

        [Required(ErrorMessage = "O código do cupom é obrigatório.")]
        [Display(Name = "Codigo", Description = "Código do cupom.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O nome do cupom é obrigatório.")]
        [Display(Name = "Nome", Description = "Nome do cupom.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de validade do cupom é obrigatória.")]
        [Display(Name = "DataValidade", Description = "Data de validade do cupom.")]
        [RegularExpression(@"^\d{2}\/\d{2}\/\d{4}$", ErrorMessage = "Data de validade inválida. Use o formato dd/MM/aaaa.")]
        public DateTime DataValidade { get; set; }

        [Required(ErrorMessage = "O desconto do cupom é obrigatório.")]
        [Display(Name = "Desconto", Description = "Desconto do cupom.")]
        public decimal Desconto { get; set; }
    }
}
