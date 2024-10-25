using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Domain.Entities;
using GhostyNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GhostyNetwork.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.Phone == phoneNumber);
        }

        public async Task<List<User>> SearchUsersAsync(string searchTerm)
        {
            var users = await _context.Users
         .Where(u => u.UserName.Contains(searchTerm) ||
                     u.FirstName.Contains(searchTerm) ||
                     u.LastName.Contains(searchTerm))
         .ToListAsync();

            return users;
        }

        public async Task<User> GetByPhoneAsync(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }
    }
}
