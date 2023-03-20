using MaratukAdmin.Models;
using MaratukAdmin.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Controllers
{
    public class BaseController : Controller
    {
        private TokenData? _tokenData = null;
        private readonly JwtTokenService _jwtTokenService;

        public BaseController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        protected TokenData? TokenData
        {
            get
            {
                if (_tokenData == null)
                {
                    _tokenData = _jwtTokenService.ExtractDataFromToken(User.Identity);
                }

                return _tokenData;
            }
        }
    }
}
