using LookDaysAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityAPIController : ControllerBase
    {
        private readonly LookdaysContext _context;
        public ActivityAPIController(LookdaysContext context)
        {
            _context = context;
        }

        // GET: api/<ActivityAPI>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivity()
        {
            if(_context.Activities == null)
            {
                return NotFound();
            }
            return await _context.Activities.ToListAsync();
        }

        // GET api/<ActivityAPI>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            if(_context.Activities == null) { return NotFound(); }
            var Activity = await _context.Activities.FindAsync(id);

            if(Activity == null)
            {
                return NotFound();
            }
            return Activity;
        }

        // POST api/<ActivityAPI>
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
            if(_context.Activities == null)
            {
                return Problem("Entity set 'LookdaysContext.Activities'  is null.");
            }
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.ActivityId }, activity);
        }

        // PUT api/<ActivityAPI>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, Activity activity)
        {
            if(id!=activity.ActivityId)
            {
                return BadRequest();
            }
            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else throw;
            }
            return NoContent();
        }

        // DELETE api/<ActivityAPI>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private bool ActivityExists(int id)
        {
            return (_context.Activities?.Any(e => e.ActivityId == id)).GetValueOrDefault();
        }
    }
}
