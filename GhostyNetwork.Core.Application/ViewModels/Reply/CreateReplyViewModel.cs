using System.ComponentModel.DataAnnotations;

namespace GhostyNetwork.Core.Application.ViewModels.Reply
{
    public class CreateReplyViewModel
    {
        [Required] public int CommentId { get; set; }

        [Required(ErrorMessage = "La repuesta al comentario no puede estar vacia.")]
        public string? Content { get; set; }
    }
}