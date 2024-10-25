using GhostyNetwork.Core.Application.ViewModels.Friendships;
using GhostyNetwork.Core.Application.ViewModels.Posts;
using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Services
{
    public interface IFriendshipService : IGenericService<AvailableFriendViewModel, FriendViewModel, Friendship>
    {
        Task<List<int>> GetFriendIdsByUserIdAsync(int userId);
        Task AddFriendByUsernameAsync(string currentUsername, string friendUsername);
        Task<List<PostViewModel>> GetPostsFromFriendsAsync(int userId);
        Task<List<AvailableFriendViewModel>> GetFriendsAsync(int userId);
        Task RemoveFriendshipAsync(int userId, int friendId);
    }
}