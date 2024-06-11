using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginJWTController : ControllerBase
    {
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

        // POST api/<LoginJWTController>
        //[HttpPost("Log-in")]
        //public async Task<ActionResult> Login(LoginDTO CurrentUser)
        //{
        //    try
        //    {
                
        //    }
        //}
    }
}
