using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Application.ViewModels.Comments
{
    public class CreateCommentViewModel
    {
        [Required] public int PostId { get; set; }

        [Required(ErrorMessage = "El contenido del comentario no puede estar vacio.")]
        [StringLength(100, ErrorMessage = "Commentario demasiado largo, deve de ser menor a 100 caracteres.")]
        public string? Content { get; set; }
    }
}