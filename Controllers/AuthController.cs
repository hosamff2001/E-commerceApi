using Ecommerce.Core.Models;
using Ecommerce.EF.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServiec _authService;

        public AuthController(IAuthServiec authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisteredModel registeredModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.Registeration(registeredModel);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiryTime, result.Id);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.SignIn(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiryTime, result.Id);
            return Ok(result);
        }
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole([FromBody] RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.AddRole(model);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Failed to add role");
            }
            return Ok(model);
        }
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken(string? refreshToken)
        {
             refreshToken = Request.Cookies["refreshToken"];

            AuthModel result = await _authService.RefrachToken(token: refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiryTime,result.Id);

            return Ok(result);
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires,string uid)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            Response.Cookies.Append("User_id", uid, cookieOptions);
            //Response.Cookies.Append("User_id", , cookieOptions);

        }
    }
}
