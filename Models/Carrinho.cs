using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Carrinho
    {
        [Display(Name = "IdCarrinho", Description = "Identificador do carrinho.")]
        public int IdCarrinho { get; set; }

        [Display(Name = "ProdutoId", Description = "Identificador do produto.")]
        public int ProdutoId { get; set; }

        [Display(Name = "Produto", Description = "Produto adicionado ao carrinho.")]
        public Produto Produto { get; set; }

        [Display(Name = "Usuário", Description = "Usuário proprietário do carrinho.")]
        [Required(ErrorMessage = "O Id do usuário é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Preço unitário inválido. Use até 2 casas decimais.")]
        public decimal PrecoUnitario { get; set; }

        [Display(Name = "Quantidade", Description = "Quantidade de produtos no carrinho.")]
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quantidade deve ser um número inteiro maior que zero.")]
        public int Quantidade { get; set; }

        [Display(Name = "Subtotal", Description = "Subtotal do carrinho.")]
        [Required(ErrorMessage = "O subtotal é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Subtotal inválido. Use até 2 casas decimais.")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Total", Description = "Total do carrinho.")]
        [Required(ErrorMessage = "O total é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Total inválido. Use até 2 casas decimais.")]
        public decimal Total { get; set; }

        [Display(Name = "UsuarioId", Description = "Identificador do usuário proprietário do carrinho.")]
        [Required(ErrorMessage = "O Id do usuário é obrigatório.")]
        public int UsuarioId { get; set; }

        [Display(Name = "Usuario", Description = "Usuário proprietário do carrinho.")]
        public Usuario Usuario { get; set; }
    }

}
