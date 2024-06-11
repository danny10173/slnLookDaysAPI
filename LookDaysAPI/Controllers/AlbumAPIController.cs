using LookDaysAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumAPIController : ControllerBase
    {
        private readonly LookdaysContext _context;
        public AlbumAPIController(LookdaysContext context)
        {
            _context = context;
        }
        // GET: api/<AlbumAPIController>
        [HttpGet]
        public async Task<ActionResult<List<ActivitiesAlbum>>> GetAlbum()
        {
            if (_context.ActivitiesAlbums == null)
            {
                return NotFound();
            }
            return await _context.ActivitiesAlbums.ToListAsync();
        }

        // GET api/<AlbumAPIController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AlbumAPIController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AlbumAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AlbumAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
