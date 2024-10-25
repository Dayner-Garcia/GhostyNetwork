using GhostyNetwork.Core.Application.ViewModels.Reply;

namespace GhostyNetwork.Core.Application.ViewModels.Comments
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? UserProfilePicture { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ReplyViewModel> Replies { get; set; } = new List<ReplyViewModel>();
    }
}