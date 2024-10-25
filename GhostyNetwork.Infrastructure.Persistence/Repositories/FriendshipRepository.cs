using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Domain.Entities;
using GhostyNetwork.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GhostyNetwork.Infrastructure.Persistence.Repositories
{
    public class FriendshipRepository :  GenericRepository<Friendship>, IFriendshipRepository
    {
        private readonly ApplicationContext _dbContext;

        public FriendshipRepository(ApplicationContext dbContext) :  base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<int>> GetFriendIdsByUserIdAsync(int userId)
        {
            return await _dbContext.Friendships
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> AreFriendsAsync(int userId, int friendId)
        {
            return await _dbContext.Friendships
                .AnyAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                               (f.UserId == friendId && f.FriendId == userId));
        }

        public async Task AddFriendByUsernameAsync(string currentUsername, string friendUsername)
        {
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == currentUsername);
            var friendUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == friendUsername);

            if (currentUser != null && friendUser != null && currentUser.Id != friendUser.Id)
            {
                if (!await AreFriendsAsync(currentUser.Id, friendUser.Id))
                {
                    var friendship1 = new Friendship
                    {
                        UserId = currentUser.Id,
                        FriendId = friendUser.Id
                    };

                    var friendship2 = new Friendship
                    {
                        UserId = friendUser.Id,
                        FriendId = currentUser.Id
                    };

                    await _dbContext.Friendships.AddAsync(friendship1);
                    await _dbContext.Friendships.AddAsync(friendship2);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveFriendshipAsync(int userId, int friendId)
        {
            var friendships = await _dbContext.Friendships
        .Where(f => (f.UserId == userId && f.FriendId == friendId) ||
                     (f.UserId == friendId && f.FriendId == userId))
        .ToListAsync();

            if (friendships.Any())
            {
                _dbContext.Friendships.RemoveRange(friendships);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Friendship>> GetFriendsByIdsAsync(List<int> friendIds)
        {
            return await _dbContext.Friendships
                .Where(f => friendIds.Contains(f.FriendId))
                .Distinct()
                .ToListAsync();
        }
    }
}
