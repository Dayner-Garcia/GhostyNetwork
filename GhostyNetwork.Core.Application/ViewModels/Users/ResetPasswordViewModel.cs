using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Application.ViewModels.Users
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string? UserName { get; set; }
    }
}