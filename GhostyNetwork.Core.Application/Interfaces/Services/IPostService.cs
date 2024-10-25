using GhostyNetwork.Core.Application.ViewModels.Posts;
using GhostyNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace GhostyNetwork.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<CreatePostViewModel, PostViewModel, Post>
    {
        Task<PostViewModel> GetPostByIdAsync(int postId);
        Task UpdatePost(EditPostViewModel editPostViewModel, int userId);
        Task<List<PostViewModel>> GetRecentPostsAsync();
        Task<List<PostViewModel>> GetRecentPostsAsync(int userId);
        Task CreatePostAsync(CreatePostViewModel createPostViewModel, int userId, string postImagePath);
        Task DeletePost(int postId, int userId);
        Task<string> SavePostImageAsync(IFormFile postImage);
    }
}