using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Avaliacao
    {
        [Display(Name = "IdAvaliacao", Description = "Identificador da avaliação.")]
        public int IdAvaliacao { get; set; }

        [Display(Name = "Nota", Description = "Nota da avaliação.")]
        [Required(ErrorMessage = "A nota é obrigatória.")]
        [RegularExpression(@"^[1-5]$", ErrorMessage = "Nota deve ser um número inteiro entre 1 e 5.")]
        public int Nota { get; set; }

        [Display(Name = "Comentario", Description = "Comentário sobre o produto.")]
        [Required(ErrorMessage = "O comentário é obrigatório.")]
        [RegularExpression(@"^.{1,500}$", ErrorMessage = "Comentário deve ter entre 1 e 500 caracteres.")]
        public string Comentario { get; set; }

        [Display(Name = "Data", Description = "Data da avaliação.")]
        [Required(ErrorMessage = "A data é obrigatória.")]
        [RegularExpression(@"^\d{2}\/\d{2}\/\d{4}$", ErrorMessage = "Data inválida. Use o formato dd/MM/aaaa.")]
        public DateTime Data { get; set; }

        [Display(Name = "UsuarioId", Description = "Identificador do usuário proprietário do carrinho.")]
        public int UsuarioId { get; set; }

        [Display(Name = "Usuario", Description = "Usuário proprietário da avaliação.")]
        public Usuario Usuario { get; set; }

        [Display(Name = "ProdutoId", Description = "Identificador do produto avaliado.")]
        public int ProdutoId { get; set; }

        [Display(Name = "Produto", Description = "Produto avaliado.")]
        public Produto Produto { get; set; }
    }
}
