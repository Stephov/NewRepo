using Azure;
using MaratukAdmin.Controllers.admin;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        public async Task<bool> isUserExist(string userName)
        {
            var res = await _userManager.IsUserNameExistAsync(userName);
            return res;
        }

        [HttpGet("activate")]
        [AllowAnonymous]
        public async Task Activate(int Id,string HashId)
        {

             await _userManager.ActivateUserAgency(Id,HashId);
           
        }

        [HttpGet("Approve")]
        [AllowAnonymous]
        public async Task Approve(int Id)
        {

            await _userManager.ApproveUserAgency(Id);

        }



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
        public async Task<ActionResult> AgencyUserLogin([FromBody] UserCredentialsRequest user)
        {
            try
            {
                AuthenticationResponse response = await _userManager.AgencyUserLoginAsync(user.Email, user.Password);

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
