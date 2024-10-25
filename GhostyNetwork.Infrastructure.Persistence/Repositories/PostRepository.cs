using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Domain.Entities;
using GhostyNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GhostyNetwork.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationContext _dbContext;

        public PostRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetAllPostsWithCommentsAndRepliesAsync()
        {
            return await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .OrderByDescending(p => p.CreatedBy)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsFromFriendsAsync(int userId, List<int> friendIds)
        {
            return await _dbContext.Posts
                .Where(p => friendIds.Contains(p.UserId))
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .OrderByDescending(p => p.Created)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(int userId)
        {
            return await _dbContext.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .OrderByDescending(p => p.Created)
                .ToListAsync();
        }
    }
}
