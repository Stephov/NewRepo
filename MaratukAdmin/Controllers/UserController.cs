using Azure;
using MaratukAdmin.Controllers.admin;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using static MaratukAdmin.Controllers.UserController;

namespace MaratukAdmin.Controllers
{

    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = "AdminScheme")]
    public class UserController : BaseController
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager, JwtTokenService jwtTokenService) : base(jwtTokenService)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Login admin user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        /// 


        [HttpGet("isUserExist")]
        [AllowAnonymous]
        public async Task<bool> isUserExist(string email)
        {
            var res = await _userManager.IsUserEmailExistAsync(email);
            return res;
        }
        [HttpGet("managers")]
        [AllowAnonymous]
        public async Task<List<ManagerResponse>> GetManagersAsync()
        {
            var res = await _userManager.GetManagersAsync();
            return res;
        }


        [HttpGet("activate")]
        [AllowAnonymous]
        public async Task<bool> Activate(int Id, string HashId)
        {

            var res = await _userManager.ActivateUserAgency(Id, HashId);
            return res;
        }

        [HttpGet("approve")]
        public async Task<bool> Approve(int Id, int statusId)
        {
            var res = await _userManager.ApproveUserAgency(Id, statusId);
            return res;
        }


        [HttpGet("GetAgencyUserForAcc")]
        public async Task<ActionResult> GetAgencyUsersForAcc()
        {
            var result = await _userManager.GetAgencyAgentsForAccAsync();

            return Ok(result);
        }


        [HttpGet("GetUsersByRole")]
        public async Task<ActionResult> GetUsersByRole([FromQuery][Required] Enums.enumRoles  role = Enums.enumRoles.All)
        {
            string? selectedRole = (role == Enums.enumRoles.All) ? null : role.ToString();

            var result = await _userManager.GetUsersByRoleAsync(selectedRole);

            return Ok(result);
        }

        //[JsonConverter(typeof(StringEnumConverter))]
        //public enum enumRoles
        //{
        //    [Display(Name = "All")]
        //    All,
        //    [Display(Name = "Accountant")]
        //    Accountant,
        //    [Display(Name = "Admin")]
        //    Admin,
        //    [Display(Name = "Hotel")]
        //    Hotel,
        //    [Display(Name = "Manager")]
        //    Manager
        //}

        [HttpGet("auth/profile")]
        [AllowAnonymous]
        public async Task<ActionResult> getUser()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];


            var result = _userManager.CheckUser(authHeader);
            if (result is null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserCredentialsRequest user)
        {
            try
            {
                AuthenticationResponse response = await _userManager.LoginAsync(user.Email, user.Password);

                if (response is null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost("AgencyUserLogin")]
        [AllowAnonymous]
        public async Task<ActionResult> AgencyUserLogin([FromBody] AgencyUserLoginCredentialsRequest user)
        {
            try
            {
                var response = await _userManager.AgencyUserLoginAsync(user.Email, user.Password);

                if (response is null)
                {
                    return BadRequest();
                }
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        /// <summary>
        /// Register new admin user
        /// </summary>
        /// <param name="email">Register user email</param>
        /// <param name="password">register user password</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserCredentialsRequest user)
        {
            try
            {
                await _userManager.RegisterAsync(user.Email, user.Password, user.UserName, user.FullName);

                return Ok(new { Email = user.Email, Password = user.Password });
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }


        /// <summary>
        /// Register new admin user
        /// </summary>
        /// <param name="email">Register user email</param>
        /// <param name="password">register user password</param>
        /// <returns></returns>
        [HttpPost("registerAgency")]
        public async Task<ActionResult> RegisterAgency([FromBody] AgencyUserCredentialsRequest user)
        {
            try
            {
                await _userManager.RegisterAgencyUserAsync(user);

                return Ok(new { Email = user.Email, Password = user.Password });
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        /// <summary>
        /// Register new admin user
        /// </summary>
        /// <param name="email">Register user email</param>
        /// <param name="password">register user password</param>
        /// <returns></returns>
        [HttpPost("registerAgencyAgent")]
        public async Task<ActionResult> RegisterAgency([FromBody] AgencyAgentCredentialsRequest agent)
        {
            try
            {
                await _userManager.RegisterAgencyAgentAsync(agent);

                return Ok(new { Email = agent.Email, Password = agent.Password });
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost("updateAgencyAgent")]
        public async Task<ActionResult> UpdateAgency([FromBody] AgencyAgentUpdateCredentialsRequest agent)
        {
            try
            {
                await _userManager.UpdateAgencyAgentAsync(agent);

                return Ok(new { Email = agent.Email, Password = agent.Password });
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }


        [HttpGet("GetAgencyAgent/{itn:int}")]
        public async Task<ActionResult> GetAgencyAgentAsync(int itn)
        {
            try
            {
                var res = await _userManager.GetAgencyAgentByItnAsync(itn);

                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpGet("GetAgencyAgentById/{agentId:int}")]
        public async Task<ActionResult> GetAgencyAgentByIdAsync(int agentId)
        {
            try
            {
                var res = await _userManager.GetAgencyAgentByIdAsync(agentId);

                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpDelete("DeleteAgencyAgent/{agentId:int}")]
        public async Task<ActionResult> DeleteAgencyAgentAsync(int agentId)
        {
            try
            {
                var res = await _userManager.DeleteAgentAsync(agentId);

                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        /// <summary>
        /// Change admin user password
        /// </summary>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns></returns>
        [HttpPut("updatePasswordByEmail")]
        public async Task<ActionResult> ChangePassword(string newPassword1, string newPassword2, string email, string hash)
        {
            try
            {
                bool isChanged = await _userManager.ChangePassword(newPassword1, newPassword2, email, hash);

                return Ok(isChanged);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Change admin user password
        /// </summary>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns></returns>
        [HttpPut("ForgotPassword")]
        public async Task<ActionResult> ChangePassword(string email)
        {

            bool isChanged = await _userManager.ForgotPassword(email);

            return Ok(isChanged);

        }


        /// <summary>
        /// Change admin user password
        /// </summary>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns></returns>
        [HttpPut("password")]
        public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                bool isChanged = await _userManager.ChangePassword(oldPassword, newPassword, TokenData);

                return Ok(isChanged);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        /// <param name="token">Refresh token</param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken(string token)
        {
            try
            {
                AuthenticationResponse response = await _userManager.RefreshToken(token);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
