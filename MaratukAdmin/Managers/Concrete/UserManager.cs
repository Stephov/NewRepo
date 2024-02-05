using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Services;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace MaratukAdmin.Managers.Concrete
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public UserManager(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }





        public async Task<bool> ChangePassword(string oldPassword, string newPassword, TokenData tokenData)
        {
            PasswordValidator.ValidatePassword(newPassword);

            User user = await _userRepository.GetUserByIdAsync(tokenData.UserId);

            if (user == null) throw new KeyNotFoundException("Invalid user");

            string passwordHash = PasswordHasher.HashPassword(oldPassword, user.PasswordSalt);

            if (passwordHash != user.Password) throw new ArgumentException("Invalid old password");

            string salt = PasswordHasher.GetSalt();
            string newPasswordHash = PasswordHasher.HashPassword(newPassword, salt);
            user.Password = newPasswordHash;
            user.PasswordSalt = salt;

            try
            {
                await _userRepository.UpdateUser();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        public async Task<bool> ForgotPassword(string email)
        {


            var user = await _userRepository.GetAgencyUserAsync(email);

            if(user == null)
            {
                return false;
            }

            string hash = PasswordHasher.GenerateHashForEmail(email);
            try
            {
                MailService.SendEmail(email, "Forgot Mail", $"please update new password, http://13.51.156.155/user/updatePassword?email={email}&HashId={hash} ");
                return true;
            }
            catch
            {
                return false;
            }


        }

        public async Task<List<AgencyAgentResponse>> GetAgencyAgentByItnAsync(int itn)
        {
            var respones = new List<AgencyAgentResponse>();

            var response =  await _userRepository.GetAgencyUsersAsync(itn);

            foreach(var agencyUser in response)
            {
                AgencyAgentResponse agent = new AgencyAgentResponse()
                {
                    Id = agencyUser.Id,
                    FullName = agencyUser.FullName,
                    Email = agencyUser.Email,
                    PhoneNumber = agencyUser.PhoneNumber1
                };
            respones.Add(agent);
            }

            return respones;
        }


        public async Task<AgencyAgentResponse> GetAgencyAgentByIdAsync(int agentId)
        {
            

            var response = await _userRepository.GetAgencyUsersByIdAsync(agentId);

            if(response != null)
            {
                return new AgencyAgentResponse()
                {
                    Id = response.Id,
                    FullName = response.FullName,
                    Email = response.Email,
                    PhoneNumber = response.PhoneNumber1
                };
            }

            return null;
        }


        public IdentityUserInfo CheckUser(string token)
        {
            var isValidUser = _jwtTokenService.UserIdentity(token);

            if (isValidUser != null)
            {
                var user = JWTUserExtractor.GetUserInfo(isValidUser);

                return user;
            }

            return null;



        }

        public async Task<AuthenticationResponse> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserAsync(email);


            if (user == null) throw new KeyNotFoundException("User not found");

            await _userRepository.RevokeExpiredRefreshTokens(user.Id);
            string passwordHash = PasswordHasher.HashPassword(password, user.PasswordSalt);

            if (passwordHash == user.Password)
            {
                var tokenData = new TokenData()
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                };
                string? refreshToken = await _jwtTokenService.GetRefreshToken(tokenData);
                string? accessToken = _jwtTokenService.GetAccessToken(tokenData);

                var response = new AuthenticationResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = (int)TimeSpan.FromMinutes(1).TotalSeconds
                };

                var refresh = new RefreshToken()
                {
                    Token = refreshToken,
                    IsRevoked = false,
                    UserId = user.Id,
                    ExpireDate = DateTime.UtcNow.AddDays(90)
                };

                await _userRepository.AddRefreshToken(refresh);
                response.name = user.Name;
                response.Role= user.Role;
                response.Id = user.Id;
                return response;
            }

            throw new ArgumentException("Email or password is wrong");
        }

        public async Task<AuthenticationResponse> RefreshToken(string token)
        {
            var refreshToken = await UpdateRefreshTokens(token);

            if (refreshToken != null)
            {
                var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId);

                if (user != null)
                {
                    var tokenData = new TokenData()
                    {
                        UserId = user.Id,
                        Email = user.Email,
                    };

                    string? accessToken = _jwtTokenService.GetAccessToken(tokenData);

                    var response = new AuthenticationResponse()
                    {
                        AccessToken = accessToken,
                        RefreshToken = token,
                        ExpiresIn = (int)TimeSpan.FromMinutes(60).TotalSeconds
                    };

                    return response;
                }

                throw new ArgumentException("Invalid refresh token");
            }
            else
            {
                throw new ArgumentException("Invalid refresh token");
            }
        }

        public async Task RegisterAgencyUserAsync(AgencyUserCredentialsRequest agencyUserCredentialsRequest)
        {



            string salt = PasswordHasher.GetSalt();
            string passwordHash = PasswordHasher.HashPassword(agencyUserCredentialsRequest.Password, salt);

            var agencyUser = new AgencyUser()
            {
                AgencyName = agencyUserCredentialsRequest.AgencyName,
                FullCompanyName = agencyUserCredentialsRequest.FullCompanyName,
                CountryId = agencyUserCredentialsRequest.CountryId,
                CityId = agencyUserCredentialsRequest.CityId,
                CompanyLocation = agencyUserCredentialsRequest.CompanyLocation,
                CompanyLegalAddress = agencyUserCredentialsRequest.CompanyLegalAddress,
                Itn = agencyUserCredentialsRequest.Itn,
                BankAccountNumber = agencyUserCredentialsRequest.BankAccountNumber,
                PhoneNumber1 = agencyUserCredentialsRequest.PhoneNumber1,
                PhoneNumber2 = agencyUserCredentialsRequest.PhoneNumber2,
                FullName = agencyUserCredentialsRequest.FullName,
                Email = agencyUserCredentialsRequest.Email,
                HashId = GenerateHashId(agencyUserCredentialsRequest.FullName, 10),
                Password = passwordHash,
                PasswordSalt = salt,
                IsActivated = false,
                IsAproved = false,
                Role = "Admin"
            };

            try
            {
                await _userRepository.CreateAgencyUserAsync(agencyUser);

                MailService.SendEmail(agencyUser.Email, "Activation Mail", $"please activate email, http://16.171.143.175:3000/user/activate?Id={agencyUser.Id}&HashId={agencyUser.HashId} ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task RegisterAgencyAgentAsync(AgencyAgentCredentialsRequest agencyAgentCredentialsRequest)
        {



            string salt = PasswordHasher.GetSalt();
            string passwordHash = PasswordHasher.HashPassword(agencyAgentCredentialsRequest.Password, salt);

            var agencyUser = new AgencyUser()
            {

                Itn = agencyAgentCredentialsRequest.AgencyItn,
                PhoneNumber1 = agencyAgentCredentialsRequest.PhoneNumber1,
                PhoneNumber2 = agencyAgentCredentialsRequest.PhoneNumber2,
                FullName = agencyAgentCredentialsRequest.FullName,
                Email = agencyAgentCredentialsRequest.Email,
                HashId = GenerateHashId(agencyAgentCredentialsRequest.FullName, 10),
                Password = passwordHash,
                PasswordSalt = salt,
                IsActivated = true,
                IsAproved = true,
                Role = "Agent"
            };

            try
            {
                await _userRepository.CreateAgencyUserAsync(agencyUser);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AgencyUser> UpdateAgencyAgentAsync(AgencyAgentUpdateCredentialsRequest agencyAgentUpdateCredentialsRequest)
        {
            if(agencyAgentUpdateCredentialsRequest.Password != null)
            {


                string salt = PasswordHasher.GetSalt();
                string passwordHash = PasswordHasher.HashPassword(agencyAgentUpdateCredentialsRequest.Password, salt);
                var res = await _userRepository.UpdateAgencyUser(agencyAgentUpdateCredentialsRequest, salt, passwordHash);
                return res;
            }
            var result = await _userRepository.UpdateAgencyUser(agencyAgentUpdateCredentialsRequest, null, null);

            return result;
        }


        public async Task RegisterAsync(string email, string password, string userName, string fullName)
        {
            bool isUserExists = await _userRepository.IsUserExistsAsync(email);

            if (isUserExists)
            {
                throw new ArgumentException("User with this Email already exists");
            }

            string salt = PasswordHasher.GetSalt();
            string passwordHash = PasswordHasher.HashPassword(password, salt);
            var user = new User()
            {
                Email = email,
                UserName = userName,
                Name = fullName,
                Password = passwordHash,
                PasswordSalt = salt
            };

            try
            {
                await _userRepository.CreateUserAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<RefreshToken> UpdateRefreshTokens(string token)
        {
            var refreshToken = await _userRepository.ValidateRefreshToken(token);

            if (refreshToken != null)
            {
                if (refreshToken.ExpireDate <= DateTime.UtcNow)
                {
                    refreshToken.IsRevoked = true;
                }

                await _userRepository.UpdateRefreshToken(refreshToken);

            }

            return refreshToken;
        }


        public static string GenerateHashId(string input, int byteLength)
        {
            if (byteLength <= 0 || byteLength > 32) // SHA256 produces a 32-byte hash (256 bits).
                throw new ArgumentException("Byte length must be between 1 and 32");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Take the first 'byteLength' bytes of the hash
                byte[] truncatedHash = new byte[byteLength];
                Buffer.BlockCopy(hashBytes, 0, truncatedHash, 0, byteLength);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in truncatedHash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public async Task<bool> IsUserEmailExistAsync(string email)
        {
            //to is email exist
            var res = await _userRepository.IsUserEmailExistsAsync(email);
            return res;
        }

        public async Task<bool> ActivateUserAgency(int Id, string HashId)
        {
            var res = await _userRepository.ActivateUserAgency(Id, HashId);
            return res;
        }

        public async Task<bool> ApproveUserAgency(int Id)
        {
            var ras = await _userRepository.ApproveUserAgency(Id);
            return ras;
        }

        public async Task<List<AgencyAgentResponseForAcc>> GetAgencyAgentsForAccAsync()
        {
            List<AgencyAgentResponseForAcc> agents = new List<AgencyAgentResponseForAcc>();
            var res = await _userRepository.GetAllAgencyUserAsync();
            foreach(var agent in res)
            {
                AgencyAgentResponseForAcc user = new AgencyAgentResponseForAcc();




                user.Id = agent.Id;
                user.AgencyName = agent.AgencyName;
                user.FullCompanyName = agent.FullCompanyName;
                user.CompanyLocation = agent.CompanyLocation;
                user.CompanyLegalAddress = agent.CompanyLegalAddress;
                user.Itn = agent.Itn;
                user.PhoneNumber1 = agent.PhoneNumber1;
                user.PhoneNumber2 = agent.PhoneNumber2;
                user.FullName = agent.FullName;
                user.email = agent.Email;
                user.IsApproved = agent.IsAproved;
                 agents.Add(user);   
            }

            return agents.OrderBy(x=> x.IsApproved).ToList();
        }

        public async Task<AgencyAuthenticationResponse> AgencyUserLoginAsync(string email, string password)
        {

            var user = await _userRepository.GetAgencyUserAsync(email);


            if (user == null) throw new KeyNotFoundException("User not found");


            await _userRepository.RevokeExpiredRefreshTokens(user.Id);
            string passwordHash = PasswordHasher.HashPassword(password, user.PasswordSalt);

            if (passwordHash == user.Password)
            {

                if (!user.IsActivated) throw new KeyNotFoundException("User not Activated");
                if (!user.IsAproved) throw new KeyNotFoundException("User not Approved by Admin");

                var tokenData = new TokenData()
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Name = user.FullName,
                };
                string? refreshToken = await _jwtTokenService.GetRefreshToken(tokenData);
                string? accessToken = _jwtTokenService.GetAccessToken(tokenData);

                var response = new AgencyAuthenticationResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = (int)TimeSpan.FromMinutes(60).TotalSeconds
                };

                var refresh = new RefreshToken()
                {
                    Token = refreshToken,
                    IsRevoked = false,
                    UserId = user.Id,
                    ExpireDate = DateTime.UtcNow.AddDays(90)
                };

                await _userRepository.AddRefreshToken(refresh);
                response.name = user.FullName;
                response.Role = user.Role;
                response.Itn = user.Itn;
                response.AgentId = user.Id;
                return response;
            }

            throw new ArgumentException("Email or password is wrong");


            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(string newPassword1, string newPassword2, string email, string hash)
        {
            string emailHash = PasswordHasher.GenerateHashForEmail(email);
            if (hash != emailHash)
            {
                throw new ArgumentException("hash is not valid");
            }
            PasswordValidator.ValidatePassword(newPassword1);

            var user = await _userRepository.GetAgencyUserAsync(email);

            if (user == null) throw new KeyNotFoundException("Invalid user");

            string salt = PasswordHasher.GetSalt();
            string newPasswordHash = PasswordHasher.HashPassword(newPassword1, salt);
            user.Password = newPasswordHash;
            user.PasswordSalt = salt;


            try
            {
                await _userRepository.UpdateUser();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<ManagerResponse>> GetManagersAsync()
        { var resp = new List<ManagerResponse>();
           var result =  await _userRepository.GetManagersAsync("manager");
            foreach(var manager in result)
            {
                ManagerResponse response = new ManagerResponse();
                response.Id = manager.Id;
                response.Name = manager.Name;
                resp.Add(response);
            }
            
            return resp;
        }

        public async Task<bool> DeleteAgentAsync(int agentId)
        {
            return await _userRepository.DeleteAgentAsync(agentId);
        }
    }
}
