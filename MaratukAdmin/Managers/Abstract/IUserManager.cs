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
        Task<List<AgencyAgentResponseForAcc>> GetAgencyAgentsForAccAsync();
        Task<AgencyAgentResponseForAcc> GetAgencyAgentsForAccAsyncById(int agentId);
        Task<bool> DeleteAgentAsync(int agentId);
        Task<AgencyUser> UpdateAgencyAgentAsync(AgencyAgentUpdateCredentialsRequest agencyAgentUpdateCredentialsRequest);
        Task<bool> ForgotPassword(string email);
        Task<List<ManagerResponse>> GetManagersAsync();
        Task<AgencyAgentResponse> GetAgencyAgentByIdAsync(int agentId);
        Task<AgencyAuthenticationResponse> AgencyUserLoginAsync(string email, string password);
        Task RegisterAgencyAgentAsync(AgencyAgentCredentialsRequest agencyAgentCredentialsRequest);
        IdentityUserInfo CheckUser(string token);
        Task<List<User>> GetUsersByRoleAsync(string? role);

        Task<bool> ActivateUserAgency(int Id, string HashId);
        Task<bool> ApproveUserAgency(int Id,int status);
        Task RegisterAsync(string email, string password, string userName,string fullName);
        Task RegisterAgencyUserAsync(AgencyUserCredentialsRequest agencyUserCredentialsRequest);
        Task<bool> ChangePassword(string oldPassword, string newPassword, TokenData tokenData);
        Task<bool> ChangePassword(string newPassword1, string newPassword2,string email,string hash);
        Task<bool> IsUserEmailExistAsync(string email);
        Task<AuthenticationResponse> RefreshToken(string token);

        Task<List<AgencyAgentResponse>> GetAgencyAgentByItnAsync(int itn);
    }
}
