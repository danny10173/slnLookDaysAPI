using LookDaysAPI.DataAccess;
using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using LookDaysAPI.Models.Service;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<LoginJWTController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginJWTController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //POST api/<LoginJWTController>
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
                    string JWT = "Bearer " + token;
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
    }
}
