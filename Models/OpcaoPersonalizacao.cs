using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class OpcaoPersonalizacao
    {
        public int IdOpcao { get; set; }

        // [Required]
        // public TipoOpcao Tipo { get; set; }

        [Required]
        public string Opcao { get; set; }

        [Required]
        public decimal PrecoExtra { get; set; }
    }
}
