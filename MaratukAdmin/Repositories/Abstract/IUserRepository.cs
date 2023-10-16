using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string email);
        Task<List<User>> GetManagersAsync(string role);
        Task<AgencyUser> GetAgencyUserAsync(string email);
        Task CreateUserAsync(User user);
        Task CreateAgencyUserAsync(AgencyUser agencyUser);
        Task<bool> ActivateUserAgency(int Id,string HashId);
        Task<bool> ApproveUserAgency(int Id);
        Task<bool> IsUserExistsAsync(string email);
        Task<bool> IsUserEmailExistsAsync(string email);
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateUser();
        Task<RefreshToken> ValidateRefreshToken(string token);
        Task UpdateRefreshToken(RefreshToken refreshTokens);
        Task AddRefreshToken(RefreshToken refresh);
        Task RevokeExpiredRefreshTokens(int userId);
    }
}
