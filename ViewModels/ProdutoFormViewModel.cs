using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class ProdutoFormViewModel
    {
        public int IdProduto { get; set; }  

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter de 2 a 50 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        public string Preco { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma categoria.")]
        public int CategoriaId { get; set; }

        public IFormFile? ImagemArquivo { get; set; }
    }
}