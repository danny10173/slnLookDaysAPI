using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LookDaysAPI.Models.Service
{
    // JwtGenerator 類別用於生成 JWT Token
    public class JwtGenerator
    {
        private readonly IConfiguration _config;

        // 建構函數，接收 IConfiguration 以讀取配置
        public JwtGenerator(IConfiguration config)
        {
            _config = config;
        }

        // 方法用於生成 JWT Token
        public string GenerateJwtToken(string username, string role, int id)
        {
            // 建立一個 Claim 列表，包含用戶名、角色和用戶 ID
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };

            // 從配置中獲取加密密鑰，並創建 SymmetricSecurityKey
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("JwtSettings:Key").Value!));

            // 使用密鑰創建簽名憑證
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // 創建 JWT Token
            var token = new JwtSecurityToken(
                issuer: _config.GetSection("JwtSettings:Issuer").Value,  // 設置發行者
                audience: _config.GetSection("JwtSettings:Audience").Value,  // 設置受眾
                claims: claims,  // 添加聲明
                expires: DateTime.UtcNow.AddDays(1),  // 設置過期時間
                signingCredentials: creds  // 設置簽名憑證
            );

            // 使用 JwtSecurityTokenHandler 寫出 JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);

            // 返回生成的 JWT Token
            return jwt;
        }



    }
}
