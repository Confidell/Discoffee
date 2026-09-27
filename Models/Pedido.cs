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

        [Display(Name = "IdProduto", Description = "Identificador do produto.")]
        [Required(ErrorMessage = "O identificador do produto é obrigatório.")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Display(Name = "Subtotal", Description = "Subtotal do pedido.")]
        [Required(ErrorMessage = "O subtotal é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Subtotal inválido. Use até 2 casas decimais.")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Total", Description = "Total do pedido.")]
        [Required(ErrorMessage = "O total é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Total inválido. Use até 2 casas decimais.")]
        public decimal Total { get; set; }

        [Display(Name = "Quantidade", Description = "Quantidade de produtos.")]
        [Required(ErrorMessage = "A quantidade de produtos é obrigatória.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quantidade de produtos deve ser maior que zero.")]
        public int QuantidadeProduto { get; set; }
        
        [Display(Name = "Status", Description = "Status do pedido.")]
        public StatusPedido Status { get; set; }
        
        [Display(Name = "Tipo", Description = "Tipo do pedido.")]
        public TipoPedido Tipo { get; set; }

        [Display(Name = "IdUsuario", Description = "Identificador do usuário.")]
        public int UsuarioId { get; set; }

        [Display(Name = "Usuário", Description = "Usuário que fez o pedido.")]
        public Usuario Usuario { get; set; }

        [Display(Name = "IdPagamento", Description = "Identificador do pagamento.")]
        public int PagamentoId { get; set; }

        [Display(Name = "Pagamento", Description = "Pagamento do pedido.")]
        public Pagamento Pagamento { get; set; }
    }
}
