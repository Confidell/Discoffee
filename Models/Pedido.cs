using Discoffee.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Pedido
    {
        [Display(Name = "IdPedido", Description = "Identificador do pedido.")]
        public int IdPedido { get; set; }

        [Display(Name = "Data e Hora", Description = "Data e hora do pedido.")]
        [Required(ErrorMessage = "A data e hora são obrigatórias.")]
        [RegularExpression(@"^\d{2}\/\d{2}\/\d{4}\s\d{2}:\d{2}$", ErrorMessage = "Data e hora inválidas. Use o formato dd/MM/aaaa HH:mm.")]
        public DateTime DataHora { get; set; }

        [Display(Name = "Subtotal", Description = "Subtotal do pedido.")]
        [Required(ErrorMessage = "O subtotal é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Subtotal inválido. Use até 2 casas decimais.")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Total", Description = "Total do pedido.")]
        [Required(ErrorMessage = "O total é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Total inválido. Use até 2 casas decimais.")]
        public decimal Total { get; set; }

        [Display(Name = "Frete", Description = "Frete do pedido.")]
        [Required(ErrorMessage = "O frete é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Frete inválido. Use até 2 casas decimais.")]
        public decimal Frete { get; set; }
        
        [Display(Name = "Status", Description = "Status do pedido.")]
        public StatusPedido Status { get; set; }
        
        [Display(Name = "Tipo", Description = "Tipo do pedido.")]
        public TipoPedido Tipo { get; set; }

        public Usuario Usuario { get; set; }

        // DADOS DO PEDIDO COMPLETO
        public List<ItemPedido> Itens { get; set; } = new();
    }
}
