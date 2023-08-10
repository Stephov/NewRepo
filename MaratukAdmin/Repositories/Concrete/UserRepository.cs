using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Utils;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

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

        public async Task CreateAgencyUserAsync(AgencyUser agencyUser)
        {
            try
            {
                await _dbContext.AgencyUser.AddAsync(agencyUser);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex) { string s = ex.Message;}
           
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AgencyUser> GetAgencyUserAsync(string email)
        {
            return await _dbContext.AgencyUser.FirstOrDefaultAsync(u => u.Email == email);
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

        public async Task<bool> IsUserEmailExistsAsync(string email)
        {
            return await _dbContext.AgencyUser.AnyAsync(u => u.Email == email);
        }

        public async Task ActivateUserAgency(int Id, string HashId)
        {
            var user = await _dbContext.AgencyUser.FirstOrDefaultAsync(u => u.Id == Id && u.HashId == HashId);
            if(user != null)
            {
                user.IsActivated= true;
            }

            await _dbContext.SaveChangesAsync();

        }

        public async Task ApproveUserAgency(int Id)
        {
            var user = await _dbContext.AgencyUser.FirstOrDefaultAsync(u => u.Id == Id);
            if (user != null)
            {
                user.IsAproved = true;
            }

            await _dbContext.SaveChangesAsync();

        }
    }
}
