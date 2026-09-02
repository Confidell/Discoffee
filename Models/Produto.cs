using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string Imagem { get; set; }

        [Required]
        public bool Disponibilidade { get; set; }

        [Required]
        public string TipoCategoria { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public string Tamanho { get; set; }

        [Required]
        public decimal Preco { get; set; }
    }
}
