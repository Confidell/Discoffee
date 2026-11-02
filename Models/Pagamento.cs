using System;
using System.Collections.Generic;
using Discoffee.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Pagamento
    {
        [Display(Name = "IdPagamento", Description = "Código do pagamento.")]
        public int IdPagamento { get; set; }

        [Display(Name = "FormaPagamento", Description = "Forma de pagamento.")]
        public FormaPagamento FormaPagamento { get; set; }

        [Display(Name = "DataHora", Description = "Data e hora do pagamento.")]
        [Required(ErrorMessage = "Data e hora são obrigatórias.")]
        [RegularExpression(@"^\d{2}\/\d{2}\/\d{4}\s\d{2}:\d{2}$", ErrorMessage = "Data e hora inválidas. Use o formato dd/MM/aaaa HH:mm.")]
        public DateTime DataHora { get; set; }

        [Display(Name = "ValorPago", Description = "Valor pago pelo cliente.")]
        [Required(ErrorMessage = "Valor pago é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Valor pago inválido. Use até 2 casas decimais.")]
        public decimal ValorPago { get; set; }

        [Display(Name = "Status", Description = "Status do pagamento.")]
        [Required(ErrorMessage = "Status é obrigatório.")]
        [RegularExpression(@"^(Pendente|Aprovado|Recusado|Estornado)$", ErrorMessage = "Status deve ser Pendente, Aprovado, Recusado ou Estornado.")]
        public string Status { get; set; }

        [Display(Name = "Pedido", Description = "Pedido associado ao pagamento.")]
        public Pedido Pedido { get; set; }

        [Display(Name = "Cupons", Description = "Cupons utilizados no pagamento.")]
        public ICollection<Cupom> Cupons { get; set; }

        [Display(Name = "NotaFiscal", Description = "Nota fiscal associada ao pagamento.")]
        public NotaFiscal NotaFiscal { get; set; }
    }
}
