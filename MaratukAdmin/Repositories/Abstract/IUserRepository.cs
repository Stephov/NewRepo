using MaratukAdmin.Dto.Request;
using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string email);
        Task<List<User>> GetUserAccAsync();
        Task<bool> DeleteAgentAsync(int agentId);
        Task<List<User>> GetManagersAsync(string role);
        Task<AgencyUser> GetAgencyUserAsync(string email);
        Task<List<AgencyUser>> GetAllAgencyUserAsync();
        Task CreateUserAsync(User user);
        Task CreateAgencyUserAsync(AgencyUser agencyUser);
        Task<bool> ActivateUserAgency(int Id,string HashId);
        Task<bool> ApproveUserAgency(int Id,int statusId);
        Task<bool> IsUserExistsAsync(string email);
        Task<bool> IsUserEmailExistsAsync(string email);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<AgencyUser> GetAgencyUsersByIdAsync(int agencyId);
        Task UpdateUser();
        Task<RefreshToken> ValidateRefreshToken(string token);
        Task UpdateRefreshToken(RefreshToken refreshTokens);
        Task AddRefreshToken(RefreshToken refresh);
        Task RevokeExpiredRefreshTokens(int userId);

        Task<List<AgencyUser>> GetAgencyUsersAsync(int itn);

         Task<AgencyUser> UpdateAgencyUser(AgencyAgentUpdateCredentialsRequest agencyAgentUpdateCredentialsRequest,string passwordSalt,string passwordHash);
    }
}
