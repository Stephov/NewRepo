using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Models;
using MaratukAdmin.Utils;

namespace MaratukAdmin.Managers.Abstract
{
    public interface IUserManager
    {
        Task<AuthenticationResponse> LoginAsync(string email, string password);
        Task<AuthenticationResponse> AgencyUserLoginAsync(string email, string password);

        IdentityUserInfo CheckUser(string token);

        Task ActivateUserAgency(int Id, string HashId);
        Task ApproveUserAgency(int Id);
        Task RegisterAsync(string email, string password, string userName,string fullName);
        Task RegisterAgencyUserAsync(AgencyUserCredentialsRequest agencyUserCredentialsRequest);
        Task<bool> ChangePassword(string oldPassword, string newPassword, TokenData tokenData);
        Task<bool> IsUserNameExistAsync(string userName);
        Task<AuthenticationResponse> RefreshToken(string token);
    }
}
