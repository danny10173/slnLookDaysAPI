using LookDaysAPI.DataAccess;
using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrowsingHistoryAPI : ControllerBase
    {
        private readonly LookdaysContext _context;
        private UserRepository _userRepository;
        public BrowsingHistoryAPI(LookdaysContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
        }

        private string decodeJWT(string jwt)
        {
            jwt = jwt.Replace("Bearer ", "");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(jwt);
            string? username = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return username!;
        }

        [HttpGet("GetByUser"), Authorize(Roles = "user")]
        public async Task<IActionResult> GetUserHistory()
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
                var findHis = await _context.BrowsingHistories.Where(a => a.UserId == user.UserId)
                    .Select(
                    fh => new BrowsingHistoryDTO
                    {
                        BrowsingHistoryId = fh.BrowsingHistoryId,
                        ActivityId = fh.ActivityId,
                        BrowseTime = fh.BrowseTime
                    }).Take(4).ToListAsync();
                return Ok(findHis);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("UserBrowse"), Authorize(Roles = "user")]
        public async Task<IActionResult> UserBrowse(BrowsingHistoryDTO browsingHistoryDTO)
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

                var findhis = _context.BrowsingHistories.Where(a => a.UserId == user.UserId && a.ActivityId == browsingHistoryDTO.ActivityId);
                if(findhis != null)
                {
                    _context.BrowsingHistories.Where(a => a.UserId == user.UserId && a.ActivityId == browsingHistoryDTO.ActivityId).ExecuteDelete();
                }

                BrowsingHistory browsingHistory = new BrowsingHistory
                {
                    ActivityId = browsingHistoryDTO.ActivityId,
                    UserId = user.UserId,
                    BrowseTime = DateTime.Now
                };
                await _context.BrowsingHistories.AddAsync(browsingHistory);
                await _context.SaveChangesAsync();
                return Ok(browsingHistoryDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
