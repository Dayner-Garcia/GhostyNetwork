using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Domain.Entities;
using GhostyNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GhostyNetwork.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly ApplicationContext _dbContext;

        public CommentRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _dbContext.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .ToListAsync();
        }
    }
}
