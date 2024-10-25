using GhostyNetwork.Core.Application.ViewModels.Friendships;
using GhostyNetwork.Core.Application.ViewModels.Users;
using GhostyNetwork.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GhostyNetwork.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<RegisterViewModel, UserProfileViewModel, User>
    {
        Task<UserProfileViewModel> LoginAsync(string username, string password);
        public bool IsUserLoggedIn(HttpContext httpContext);
        Task<bool> RegisterAsync(RegisterViewModel registerViewModel, string profilePicturePath);
        Task<string> SavePhotoAsync(IFormFile photo);
        Task<bool> ActivateUserAsync(string activationToken);
        Task<bool> ResetPasswordAsync(string username);
        Task<List<AvailableFriendViewModel>> SearchUsersAsync(string searchTerm);
        Task<UserProfileViewModel> GetCurrentUserProfileAsync(ClaimsPrincipal userClaims);
        Task<bool> UpdateProfileAsync(EditProfileViewModel editProfileViewModel, string profilePicturePath, ClaimsPrincipal userClaims);
        Task<bool> UserExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        Task<bool> IsPhoneNumberTakenAsync(string phoneNumber, int userId);
    }
}