using GhostyNetwork.Core.Application.ViewModels.Comments;

namespace GhostyNetwork.Core.Application.ViewModels.Posts
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserProfilePicture { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime Created { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}