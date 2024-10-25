using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllPostsWithCommentsAndRepliesAsync();
        Task<List<Post>> GetPostsFromFriendsAsync(int userId, List<int> friendIds);
        Task<List<Post>> GetPostsByUserIdAsync(int userId);
    }
}