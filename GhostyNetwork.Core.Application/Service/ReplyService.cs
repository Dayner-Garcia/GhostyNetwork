using AutoMapper;
using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Reply;
using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Service
{
    public class ReplyService : GenericService<CreateReplyViewModel, ReplyViewModel, Reply>, IReplyService
    {
        private readonly IReplyRepository _replyRepository;
        private readonly IMapper _mapper;

        public ReplyService(IReplyRepository replyRepository, IMapper mapper) : base(replyRepository, mapper)
        {
            _replyRepository = replyRepository;
            _mapper = mapper;
        }

        public async Task CreateReplyAsync(CreateReplyViewModel createReplyViewModel, int userId)
        {
            var reply = _mapper.Map<Reply>(createReplyViewModel);
            reply.UserId = userId;
            reply.Created = DateTime.UtcNow;
            await _replyRepository.AddAsync(reply);
        }
    }
}