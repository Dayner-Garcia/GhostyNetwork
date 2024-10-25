using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Domain.Entities;
using GhostyNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GhostyNetwork.Infrastructure.Persistence.Repositories
{
    public class ReplyRepository : GenericRepository<Reply>, IReplyRepository
    {
        private readonly ApplicationContext _dbContext;

        public ReplyRepository(ApplicationContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<Reply>> GetRepliesByCommentIdAsync(int commentId)
        {
            return await _dbContext.Replies
                .Where(r => r.CommentId == commentId)
                .Include(r => r.User)
                .ToListAsync();
        }
    }
}
