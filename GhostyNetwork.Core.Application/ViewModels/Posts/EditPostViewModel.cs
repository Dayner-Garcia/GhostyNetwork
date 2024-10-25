using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Application.ViewModels.Posts
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "El contenido de la publicación es obligatorio")]
        [MaxLength(1000)]
        public string? Content { get; set; }

        public IFormFile? Image { get; set; }
        public string? VideoUrl { get; set; }
    }
}