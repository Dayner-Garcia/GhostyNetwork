using GhostyNetwork.Core.Application.ViewModels.Reply;
using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Services
{
    public interface IReplyService : IGenericService<CreateReplyViewModel, ReplyViewModel, Reply>
    {
        Task CreateReplyAsync(CreateReplyViewModel createReplyViewModel, int userId);
    }
}