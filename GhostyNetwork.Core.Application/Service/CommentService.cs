using AutoMapper;
using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Comments;
using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Service
{
    public class CommentService : GenericService<CreateCommentViewModel, CommentViewModel, Comment>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
            : base(commentRepository, mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task CreateCommentAsync(CreateCommentViewModel createCommentViewModel, int userId)
        {
            var comment = _mapper.Map<Comment>(createCommentViewModel);
            comment.UserId = userId;
            comment.Created = DateTime.UtcNow;
            await _commentRepository.AddAsync(comment);
        }
    }
}