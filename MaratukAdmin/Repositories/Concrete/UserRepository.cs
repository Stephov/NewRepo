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
            catch (Exception ex) { string s = ex.Message; }

        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetUsersByRoleAsync(string? role)
        {
            return await _dbContext.Users.Where(u => u.Role == (role ?? u.Role)).ToListAsync();
        }

        public async Task<AgencyUser> GetAgencyUserAsync(string email)
        {
            return await _dbContext.AgencyUser.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<AgencyUser>> GetAgencyUsersAsync(int itn)
        {
            return await _dbContext.AgencyUser.Where(u => u.Itn == itn).ToListAsync();
        }

        public async Task<AgencyUser> GetAgencyUsersByIdAsync(int agencyId)
        {
            return await _dbContext.AgencyUser.Where(u => u.Id == agencyId).FirstOrDefaultAsync();
        }



        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetUserAccAsync()
        {
            return await _dbContext.Users.Where(u => u.Role == "Accountant").ToListAsync();
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

        public async Task<bool> ActivateUserAgency(int Id, string HashId)
        {
            var user = await _dbContext.AgencyUser.FirstOrDefaultAsync(u => u.Id == Id && u.HashId == HashId);
            if (user != null)
            {
                user.IsActivated = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else { return false; }

        }

        public async Task<AgencyUser> UpdateAgencyUser(AgencyAgentUpdateCredentialsRequest agencyAgentUpdateCredentialsRequest, string passwordSalt, string passwordHash)
        {
            var user = await _dbContext.AgencyUser.FirstOrDefaultAsync(u => u.Id == agencyAgentUpdateCredentialsRequest.AgentId);
            if (user != null)
            {
                user.FullName = agencyAgentUpdateCredentialsRequest.FullName;
                user.PhoneNumber1 = agencyAgentUpdateCredentialsRequest.PhoneNumber;
                user.Email = agencyAgentUpdateCredentialsRequest.Email;
                if (passwordSalt != null)
                {
                    user.PasswordSalt = passwordSalt;
                    user.Password = passwordHash;
                }
                await _dbContext.SaveChangesAsync();
                return user;
            }
            else { return user; }

        }

        public async Task<bool> ApproveUserAgency(int Id,int statusId)
        {
            var user = await _dbContext.AgencyUser.FirstOrDefaultAsync(u => u.Id == Id);
            if (user != null)
            {
                user.IsAproved = statusId;
                if(statusId == 1)
                {
                    user.ApprovedDate = DateTime.UtcNow;
                }
                {
                    user.RejectedDate = DateTime.UtcNow;
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<User>> GetManagersAsync(string role)
        {
            return await _dbContext.Users.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<bool> DeleteAgentAsync(int agentId)
        {
            try
            {
                var entityToDelete = await _dbContext.AgencyUser.FindAsync(agentId);

                if (entityToDelete != null)
                {
                    _dbContext.AgencyUser.Remove(entityToDelete);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return false;
        }

        public async Task<List<AgencyUser>> GetAllAgencyUserAsync()
        {
            return await _dbContext.AgencyUser
                            .Where(u => u.IsActivated && u.Role == "Admin")  
                            .ToListAsync();
        }
    }
}
