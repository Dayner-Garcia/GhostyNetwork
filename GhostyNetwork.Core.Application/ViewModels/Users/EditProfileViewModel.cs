using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Application.ViewModels.Users
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^(809|829|849)\d{8}$|^([0-9]{10})$", ErrorMessage = "El número de teléfono no es válido. Debe estar en formato de República Dominicana.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? Email { get; set; }

        public IFormFile? ProfilePhoto { get; set; }
        public string? UserProfilePicture { get; set; }

        [DataType(DataType.Password)] public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmPassword { get; set; }
    }
}