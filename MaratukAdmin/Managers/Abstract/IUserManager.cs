using MaratukAdmin.Dto.Response;
using MaratukAdmin.Models;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IUserManager
    {
        Task<AuthenticationResponse> LoginAsync(string email, string password);
        Task RegisterAsync(string email, string password);
        Task<bool> ChangePassword(string oldPassword, string newPassword, TokenData tokenData);
        Task<AuthenticationResponse> RefreshToken(string token);
    }
}
