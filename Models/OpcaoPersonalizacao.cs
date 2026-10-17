using Discoffee.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class OpcaoPersonalizacao
    {
        [Display(Name = "IdOpcao", Description = "Código da opção de personalização.")]
        public int IdOpcao { get; set; }

        [Display(Name = "Tipo", Description = "Tipo da opção de personalização.")]
        public TipoOpcao Tipo { get; set; }

        [Display(Name = "Opcao", Description = "Opção de personalização.")]
        [Required(ErrorMessage = "A opção de personalização é obrigatória.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ0-9\s]{2,50}$", ErrorMessage = "Opção deve ter entre 2 e 50 caracteres.")]
        public string Opcao { get; set; }

        [Display(Name = "PrecoExtra", Description = "Preço extra da opção de personalização.")]
        [Required(ErrorMessage = "O preço extra é obrigatório.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Preço extra inválido. Use até 2 casas decimais.")]
        public decimal PrecoExtra { get; set; }

        [Display(Name = "CategoriaId", Description = "Código da categoria.")]
        public int? CategoriaId { get; set; }
    }
}
