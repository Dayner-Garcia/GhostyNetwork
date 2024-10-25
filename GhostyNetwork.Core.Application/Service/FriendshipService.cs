using AutoMapper;
using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Friendships;
using GhostyNetwork.Core.Application.ViewModels.Posts;
using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Service
{
    public class FriendshipService : GenericService<AvailableFriendViewModel, FriendViewModel, Friendship>,
        IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public FriendshipService(IFriendshipRepository friendshipRepository, IMapper mapper,
            IPostRepository postRepository) : base(friendshipRepository, mapper)
        {
            _friendshipRepository = friendshipRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<List<int>> GetFriendIdsByUserIdAsync(int userId)
        {
            return await _friendshipRepository.GetFriendIdsByUserIdAsync(userId);
        }

        public async Task<List<AvailableFriendViewModel>> GetFriendsAsync(int userId)
        {
            var friendIds = await GetFriendIdsByUserIdAsync(userId);

            if (friendIds == null || friendIds.Count == 0)
            {
                return new List<AvailableFriendViewModel>();
            }

            var friendships = await _friendshipRepository.GetFriendsByIdsAsync(friendIds);

            var availableFriendViewModels = _mapper.Map<List<AvailableFriendViewModel>>(friendships);

            return availableFriendViewModels;
        }

        public async Task AddFriendByUsernameAsync(string currentUsername, string friendUsername)
        {
            await _friendshipRepository.AddFriendByUsernameAsync(currentUsername, friendUsername);
        }

        public async Task<List<PostViewModel>> GetPostsFromFriendsAsync(int userId)
        {
            var friendIds = await GetFriendIdsByUserIdAsync(userId);

            if (friendIds == null || friendIds.Count == 0)
            {
                return new List<PostViewModel>();
            }

            var posts = await _postRepository.GetPostsFromFriendsAsync(userId, friendIds);

            var postViewModels = _mapper.Map<List<PostViewModel>>(posts);

            return postViewModels;
        }

        public async Task RemoveFriendshipAsync(int userId, int friendId)
        {
            await _friendshipRepository.RemoveFriendshipAsync(userId, friendId);
        }
    }
}