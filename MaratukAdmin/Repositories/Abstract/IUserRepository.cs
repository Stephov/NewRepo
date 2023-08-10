using MaratukAdmin.Entities;

namespace MaratukAdmin.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string email);
        Task<AgencyUser> GetAgencyUserAsync(string email);
        Task CreateUserAsync(User user);
        Task CreateAgencyUserAsync(AgencyUser agencyUser);
        Task ActivateUserAgency(int Id,string HashId);
        Task ApproveUserAgency(int Id);
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
