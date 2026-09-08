using System.ComponentModel.DataAnnotations;

namespace Discoffee.Models
{
    public class Perfil
    {
        [Display(Name = "IdPerfil", Description = "Código do perfil.")]
        public int IdPerfil { get; set; }

        [Display(Name = "Tipo", Description = "Tipo do perfil.")]
        [Required(ErrorMessage = "O tipo é obrigatório.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s]{3,50}$", ErrorMessage = "Tipo deve conter apenas letras, entre 3 e 50 caracteres.")]
        public string Tipo { get; set; }

        [Display(Name = "Usuarios", Description = "Usuários associados ao perfil.")]
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
