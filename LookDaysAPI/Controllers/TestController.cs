using LookDaysAPI.Models.Service;
using Microsoft.AspNetCore.Mvc;

namespace LookDaysAPI.Controllers
{
    public class TestController : Controller
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("generate-token")]
        public ActionResult GenerateToken()
        {
            var jwtGenerator = new JwtGenerator(_configuration);
            var token = jwtGenerator.GenerateJwtToken("曾先生", "ss123@gmail.com", 2);
            return Ok(new { Token = token });
        }
    }
}
