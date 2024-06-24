using LookDaysAPI.DataAccess;
using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using LookDaysAPI.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // 建構函數，初始化資料庫上下文和使用者倉庫
        public LoginJWTController(LookdaysContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _userRepository = new UserRepository(_context);
        }

        // 用於註冊新用戶的 API 端點
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignupDTO signup)
        {
            try
            {
                // 使用 UserRepository 添加新用戶
                string res = await _userRepository.AddNewUser(signup);

                // 根據結果返回不同的回應
                if (res == "註冊成功") return Ok(res);
                else return BadRequest(res);
            }
            catch (Exception)
            {
                return BadRequest("Server error. Please try again later.");
            }
        }

        // 用於存儲 JWT 的類別
        public class JWT
        {
            public string token { get; set; } = string.Empty;
        }

        // 用於用戶登入的 API 端點
        [HttpPost("Log-in")]
        public async Task<ActionResult> Login(LoginDTO loginUser)
        {
            try
            {
                // 驗證用戶
                User? user = await _userRepository.AuthUser(loginUser);
                if (user == null) return BadRequest("使用者名稱或密碼錯誤");

                // 如果用戶存在，生成 JWT
                if (user != null)
                {
                    var token = (new JwtGenerator(_configuration)).GenerateJwtToken(loginUser.Username, "user", user.UserId);
                    var JWT = new JWT
                    {
                        token = "Bearer " + token
                    };
                    return Ok(JWT);
                }
                else
                {
                    return BadRequest("使用者名稱或密碼錯誤");
                }
            }
            catch (Exception)
            {
                return BadRequest("server error");
            }
        }

        // 用於用戶登入並驗證雜湊密碼的 API 端點
        [HttpPost("Log-in-hash")]
        public async Task<ActionResult> LoginWithHashPassword(LoginDTO loginUser)
        {
            try
            {
                // 驗證用戶及其雜湊密碼
                User? user = await _userRepository.AuthHashUser(loginUser);
                if (user == null) return BadRequest("使用者名稱或密碼錯誤");
                if (user.RoleId == 9) return BadRequest("你被封鎖了");

                // 如果用戶存在，生成 JWT
                if (user != null)
                {
                    var token = (new JwtGenerator(_configuration)).GenerateJwtToken(loginUser.Username, "user", user.UserId);
                    var JWT = new JWT
                    {
                        token = "Bearer " + token
                    };
                    return Ok(JWT);
                }
                else
                {
                    return BadRequest("使用者名稱或密碼錯誤");
                }
            }
            catch (Exception)
            {
                return BadRequest("server error");
            }
        }

        // 用於獲取當前用戶資訊的 API 端點，僅限授權用戶訪問
        [HttpGet("get-current-user"), Authorize(Roles = "user")]
        public async Task<IActionResult> getCurrentUser()
        {
            try
            {
                // 從請求頭中獲取 JWT
                string? jwt = HttpContext.Request.Headers["Authorization"];

                if (jwt == null || jwt == "") return BadRequest();

                // 解碼 JWT 以獲取用戶名
                string username = decodeJWT(jwt);

                if (username == null)
                {
                    return BadRequest();
                }

                // 根據用戶名從資料庫中獲取用戶資訊
                User? user = await _userRepository.GetUserbyUsername(username);

                if (user == null)
                {
                    return NotFound("使用者不存在");
                }

                // 將用戶資訊轉換為 DTO 並返回
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

        // 解碼 JWT 並提取用戶名
        private string decodeJWT(string jwt)
        {
            jwt = jwt.Replace("Bearer ", "");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(jwt);
            string? username = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return username!;
        }

        // 解碼 JWT 並提取用戶 ID
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
