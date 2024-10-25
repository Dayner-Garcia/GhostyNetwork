using AutoMapper;
using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Application.ViewModels.Posts;
using GhostyNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace GhostyNetwork.Core.Application.Service
{
    public class PostService : GenericService<CreatePostViewModel, PostViewModel, Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task CreatePostAsync(CreatePostViewModel createPostViewModel, int userId, string postImagePath)
        {
            var post = _mapper.Map<Post>(createPostViewModel);
            post.UserId = userId;
            post.ImagePath = postImagePath;
            post.VideoUrl = createPostViewModel.VideoUrl;
            await _postRepository.AddAsync(post);
        }

        public async Task<PostViewModel> GetPostByIdAsync(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            return _mapper.Map<PostViewModel>(post);
        }

        public async Task<List<PostViewModel>> GetRecentPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsWithCommentsAndRepliesAsync();
            var recentPosts = posts.OrderByDescending(p => p.Created).ToList();
            return _mapper.Map<List<PostViewModel>>(recentPosts);
        }

        public async Task<List<PostViewModel>> GetRecentPostsAsync(int userId)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(userId);
            var recentPosts = posts.OrderByDescending(p => p.Created).ToList();
            return _mapper.Map<List<PostViewModel>>(recentPosts);
        }

        public async Task<string> SavePostImageAsync(IFormFile postImage)
        {
            if (postImage == null || postImage.Length == 0)
            {
                return null;
            }

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/posts");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(postImage.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await postImage.CopyToAsync(stream);
            }

            return $"/images/posts/{fileName}";
        }

        public async Task UpdatePost(EditPostViewModel editPostViewModel, int userId)
        {
            var post = await _postRepository.GetByIdAsync(editPostViewModel.Id);

            if (post == null)
            {
                throw new Exception("Post no encontrado.");
            }

            if (post.UserId != userId)
            {
                throw new UnauthorizedAccessException("No tienes permisos para editar este post.");
            }

            post.Content = editPostViewModel.Content;
            post.VideoUrl = editPostViewModel.VideoUrl;

            if (editPostViewModel.Image != null)
            {
                var postImagePath = await SavePostImageAsync(editPostViewModel.Image);
                post.ImagePath = postImagePath;
            }

            await _postRepository.UpdateAsync(post, post.Id);
        }

        public async Task DeletePost(int postId, int userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                throw new Exception("Post no encontrado.");
            }

            if (post.UserId != userId)
            {
                throw new UnauthorizedAccessException("No tienes permisos para eliminar este post.");
            }

            await _postRepository.DeleteAsync(post);
        }
    }
}