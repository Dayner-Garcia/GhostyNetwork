using GhostyNetwork.Core.Application.ViewModels.Comments;
using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Services
{
    public interface ICommentService : IGenericService<CreateCommentViewModel, CommentViewModel, Comment>
    {
        Task CreateCommentAsync(CreateCommentViewModel createCommentViewModel, int userId);
    }
}