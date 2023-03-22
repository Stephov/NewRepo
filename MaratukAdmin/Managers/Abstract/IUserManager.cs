using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Models;
using MaratukAdmin.Utils;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IUserManager
    {
        Task<AuthenticationResponse> LoginAsync(string email, string password);

        IdentityUserInfo CheckUser(string token);

        Task RegisterAsync(string email, string password, string userName,string fullName);
        Task<bool> ChangePassword(string oldPassword, string newPassword, TokenData tokenData);
        Task<AuthenticationResponse> RefreshToken(string token);
    }
}
