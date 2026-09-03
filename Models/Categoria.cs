using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }

        [Required]
        public string NomeCategoria { get; set; }
    }
}
