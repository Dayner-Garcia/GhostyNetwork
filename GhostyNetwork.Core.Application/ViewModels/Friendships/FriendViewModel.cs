using GhostyNetwork.Core.Application.ViewModels.Posts;

namespace GhostyNetwork.Core.Application.ViewModels.Friendships
{
    public class FriendViewModel
    {
        public int FriendId { get; set; }
        public string? UserName { get; set; }
        public string? ProfilePicture { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<PostViewModel>? FriendPosts { get; set; }
        public List<AvailableFriendViewModel>? Friends { get; set; }
        public List<AvailableFriendViewModel>? SearchResults { get; set; }
    }
}