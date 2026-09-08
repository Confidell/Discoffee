using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
   public class Produto
   {
        [Display(Name = "IdProduto", Description = "Identificador do produto.")]
        public int IdProduto { get; set; }

        [Display(Name = "Nome", Description = "Nome do produto.")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s]{2,100}$",ErrorMessage = "Nome deve ter entre 2 e 100 caracteres, sem símbolos especiais.")]
        public string Nome { get; set; }


        [Display(Name = "Descrição", Description = "Descrição do produto.")]
        [RegularExpression(@"^.{0,500}$",ErrorMessage = "Descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Display(Name = "Imagem", Description = "Imagem do produto.")]
        [Required(ErrorMessage = "A imagem é obrigatória.")]
        [RegularExpression(@"^.+\.(jpg|jpeg|png|webp)$",ErrorMessage = "Imagem deve ser um arquivo .jpg, .jpeg, .png ou .webp.")]
        public string Imagem { get; set; }

        [Display(Name = "Disponibilidade", Description = "Disponibilidade do produto.")]
        [Required(ErrorMessage = "A disponibilidade é obrigatória.")]
        [RegularExpression(@"^(true|false)$",ErrorMessage = "Disponibilidade deve ser verdadeiro ou falso.")]
        public bool Disponibilidade { get; set; }

        [Display(Name = "Tipo de Categoria", Description = "Tipo de categoria do produto.")]
        [Required(ErrorMessage = "O tipo de categoria é obrigatório.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s]{2,50}$",ErrorMessage = "Tipo de categoria deve ter entre 2 e 50 caracteres.")]
        public string TipoCategoria { get; set; }

        [Display(Name = "Quantidade", Description = "Quantidade do produto.")]
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [RegularExpression(@"^\d+$",ErrorMessage = "Quantidade deve ser um número inteiro não negativo.")]
        public int Quantidade { get; set; }

        [Display(Name = "Tamanho", Description = "Tamanho do produto.")]
        [Required(ErrorMessage = "O tamanho é obrigatório.")]
        [RegularExpression(@"^(P|M|G|Pequeno|Médio|Grande)$",ErrorMessage = "Tamanho deve ser P, M, G, Pequeno, Médio ou Grande.")]
        public string Tamanho { get; set; }

        [Display(Name = "Preço", Description = "Preço do produto.")]
        [Required(ErrorMessage = "O preço é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$",ErrorMessage = "Preço inválido. Use até 2 casas decimais.")]
        public decimal Preco { get; set; }

        [Display(Name = "Categoria", Description = "Categoria do produto.")]
        public int CategoriaId { get; set; }

        [Display(Name = "Categoria", Description = "Categoria do produto.")]
        public Categoria Categoria { get; set; }

        [Display(Name = "Avaliações", Description = "Avaliações do produto.")]
        public ICollection<Avaliacao> Avaliacoes { get; set; }

        [Display(Name = "Carrinho", Description = "Carrinho de compras.")]
        public ICollection<Carrinho> Carrinhos { get; set; }

        [Display(Name = "Pedidos", Description = "Pedidos associados ao produto.")]
        public ICollection<Pedido> Pedidos { get; set; }
   }

}
