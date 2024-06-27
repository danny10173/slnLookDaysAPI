using Humanizer;
using LookDaysAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVerifyController : ControllerBase
    {
        private readonly LookdaysContext _context;
        public UserVerifyController(LookdaysContext context)
        {
            _context = context;
        }

        [HttpGet("GetVerify")]
        public async Task<IActionResult> Verifylink (string username)
        {
            try
            {
                User Vuser = new User();
                Vuser = await _context.Users.Where(a=>a.Username == username).FirstOrDefaultAsync();
                if(Vuser != null)
                {
                    Vuser.RoleId = 1;
                }
                _context.SaveChangesAsync();
                return Ok(Vuser);
            }
            catch (Exception)
            {
                return BadRequest("server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmailTest()
        {
            GMailer mailer = new GMailer();
            mailer.ToEmail = "stanley05232930@gmail.com";
            mailer.Subject = "Lookdays Verify mail";
            mailer.Body = "感謝你註冊和使用LookDays的服務.<br> 請點擊此連結來驗證你的帳號 <br> <a href='youraccount.com/verifycode=12323232'>verify</a>";
            mailer.IsHtml = true;
            mailer.Send();

            return Ok(mailer);
        }
    }
}
