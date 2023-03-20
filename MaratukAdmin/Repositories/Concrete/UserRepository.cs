using MaratukAdmin.Entities;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MaratukAdmin.Repositories.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly MaratukDbContext _dbContext;

        public UserRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRefreshToken(RefreshToken refresh)
        {
            await _dbContext.RefreshToken.AddAsync(refresh);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task RevokeExpiredRefreshTokens(int userId)
        {
            var tokens = await _dbContext.RefreshToken.Where(u => u.UserId == userId).ToListAsync();
            tokens.ForEach(u =>
            {
                if (u.ExpireDate < DateTime.UtcNow)
                {
                    u.IsRevoked = true;
                }
            });

            _dbContext.UpdateRange(tokens);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshToken.Update(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> ValidateRefreshToken(string token)
        {
            return await _dbContext.RefreshToken.FirstOrDefaultAsync(u => u.Token == token && !u.IsRevoked);
        }
    }
}
