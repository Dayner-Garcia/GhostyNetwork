using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Repositories
{
    public interface IReplyRepository : IGenericRepository<Reply>
    {
        Task<List<Reply>> GetRepliesByCommentIdAsync(int commentId);
    }
}