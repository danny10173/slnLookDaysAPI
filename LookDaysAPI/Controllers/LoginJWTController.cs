using LookDaysAPI.DataAccess;
using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using LookDaysAPI.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.JsonWebTokens;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginJWTController : ControllerBase
    {
        private readonly LookdaysContext _context;
        private UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public LoginJWTController(LookdaysContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _userRepository = new UserRepository(_context);
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(LoginDTO signup)
        {
            try
            {
                string res = await _userRepository.AddNewUser(signup);

                if (res == "註冊成功") return Ok(res);
                else return BadRequest(res);
            }
            catch (Exception)
            {
                return BadRequest("伺服器錯誤，請稍後再試");
            }
        }
        public class JWT
        {
            public string token { get; set; } = string.Empty;
        }

        [HttpPost("Log-in")]
        public async Task<ActionResult> Login(LoginDTO loginUser)
        {
            try
            {
                User? user = await _userRepository.AuthUser(loginUser);
                if (user == null) return BadRequest("Wrong Username or Password");
                if (user != null)
                {
                    var token = (new JwtGenerator(_configuration)).GenerateJwtToken(loginUser.Username, "user", user.UserId);
                    //string JWT = "token " + token;
                    var JWT = new JWT 
                    {
                        token = "Bearer "+ token
                    };
                    return Ok(JWT);
                }
                else
                {
                    return BadRequest("Wrong Username or Password");
                }
            }
            catch (Exception)
            {
                return BadRequest("server error");
            }
        }
        [HttpGet("get-current-user"), Authorize(Roles = "user")]
        public async Task<IActionResult> getCurrentUser()
        {
            try
            {
                string? jwt = HttpContext.Request.Headers["Authorization"];

                if (jwt == null || jwt == "") return BadRequest();

                string username = decodeJWT(jwt);

                if (username == null)
                {
                    return BadRequest();
                }

                User? user = await _userRepository.GetUserbyUsername(username);

                if (user == null)
                {
                    return NotFound("使用者不存在");
                }

                CurrentUserDTO currentUser = new CurrentUserDTO
                {
                    Id = user.UserId,
                    Username = user.Username,
                    Email = user.Email ?? string.Empty
                };

                return Ok(currentUser);
            }
            catch (Exception)
            {
                return BadRequest("伺服器錯誤，請稍後再試");
            }
        }
        private string decodeJWT(string jwt)
        {
            jwt = jwt.Replace("Bearer ", "");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(jwt);
            string? username = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return username!;
        }
        private string decodeJwtId(string jwt)
        {
            jwt = jwt.Replace("Bearer ", "");

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(jwt);

            string? id = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            return id!;
        }
    }
}
