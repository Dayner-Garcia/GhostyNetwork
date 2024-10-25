using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Repositories
{
    public interface IFriendshipRepository : IGenericRepository<Friendship>
    {
        Task<List<int>> GetFriendIdsByUserIdAsync(int userId);
        Task<List<Friendship>> GetFriendsByIdsAsync(List<int> friendIds);
        Task<bool> AreFriendsAsync(int userId, int friendId);
        Task AddFriendByUsernameAsync(string currentUsername, string friendUsername);
        Task RemoveFriendshipAsync(int userId, int friendId);
    }
}