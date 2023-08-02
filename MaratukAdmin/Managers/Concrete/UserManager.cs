using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Services;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Identity;
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
                response.name = user.Name;
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
            /*           bool isUserExists = await _userRepository.IsUserExistsAsync(agencyUserCredentialsRequest.Email);

                       if (isUserExists)
                       {
                           throw new ArgumentException("User with this Email already exists");
                       }*/

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
                UserName = agencyUserCredentialsRequest.UserName,
                Email = agencyUserCredentialsRequest.Email,
                HashId = GenerateHashId(agencyUserCredentialsRequest.UserName, 10),
                Password = passwordHash,
                PasswordSalt = salt,
                IsActived = false,
                IsAproved = false
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

        public async Task<bool> IsUserNameExistAsync(string userName)
        {
            var res = await _userRepository.IsUserNameExistsAsync(userName);
            return res;
        }

        public async Task ActivateUserAgency(int Id, string HashId)
        {
           await _userRepository.ActivateUserAgency(Id, HashId);
        }

        public async Task ApproveUserAgency(int Id)
        {
            await _userRepository.ApproveUserAgency(Id);
        }

        public async Task<AuthenticationResponse> AgencyUserLoginAsync(string email, string password)
        {

            var user = await _userRepository.GetAgencyUserAsync(email);


            if (user == null) throw new KeyNotFoundException("User not found");
           

            await _userRepository.RevokeExpiredRefreshTokens(user.Id);
            string passwordHash = PasswordHasher.HashPassword(password, user.PasswordSalt);

            if (passwordHash == user.Password)
            {

                if (!user.IsActived) throw new KeyNotFoundException("User not Activated");
                if (!user.IsAproved) throw new KeyNotFoundException("User not Approved by Admin");

                var tokenData = new TokenData()
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Name = user.UserName,
                };
                string? refreshToken = await _jwtTokenService.GetRefreshToken(tokenData);
                string? accessToken = _jwtTokenService.GetAccessToken(tokenData);

                var response = new AuthenticationResponse()
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
                response.name = user.UserName;
                return response;
            }

            throw new ArgumentException("Email or password is wrong");
            

            throw new NotImplementedException();
        }
    }
}
