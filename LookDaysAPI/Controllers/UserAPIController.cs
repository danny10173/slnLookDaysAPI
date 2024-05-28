using LookDaysAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly LookdaysContext _context;
        public UserAPIController(LookdaysContext context)
        {
            _context = context;
        }

        // GET: api/<UserAPIController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET api/<UserAPIController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null) { return NotFound(); }
            var User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }
            return User;
        }

        // POST api/<UserAPIController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
