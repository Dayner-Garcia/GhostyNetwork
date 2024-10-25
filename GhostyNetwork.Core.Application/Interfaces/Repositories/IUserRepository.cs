using GhostyNetwork.Core.Domain.Entities;

namespace GhostyNetwork.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        Task<List<User>> SearchUsersAsync(string searchTerm);
        Task<User> GetByPhoneAsync(string phone);
    }
}