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

        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Imagem")]
        public string Imagem { get; set; } = string.Empty; 

        [Display(Name = "Disponibilidade")]
        public bool Disponibilidade { get; set; }

        [Display(Name = "Tamanho")]
        public string Tamanho { get; set; } = string.Empty;

        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

       
        //public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        //public ICollection<Carrinho> Carrinhos { get; set; } = new List<Carrinho>();
        //public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}