using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    // A validação do formulário fica no ProdutoFormViewModel.
    public class Produto
    {
        [Display(Name = "Id do produto")]
        public int IdProduto { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Descrição", Description = "Descrição do produto.")]
        [RegularExpression(@"^.{0,500}$",ErrorMessage = "Descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Display(Name = "Imagem")]
        public string Imagem { get; set; } = string.Empty; 

        [Display(Name = "Disponibilidade")]
        public bool Disponibilidade { get; set; }

        [Display(Name = "Quantidade", Description = "Quantidade do produto.")]
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [RegularExpression(@"^\d+$",ErrorMessage = "Quantidade deve ser um número inteiro não negativo.")]
        public int Quantidade { get; set; }

        [Display(Name = "Preço", Description = "Preço do produto.")]
        [Required(ErrorMessage = "O preço é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Preço inválido. Use até 2 casas decimais.")]
        public decimal Preco { get; set; }

        [Display(Name = "Categoria", Description = "Categoria do produto.")]
        public Categoria Categoria { get; set; }
   }
}

