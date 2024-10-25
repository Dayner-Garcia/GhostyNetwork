using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
    }
}