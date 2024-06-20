using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using System.Threading.Tasks;
using System.Linq;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly LookdaysContext _context;

        public FavoritesController(LookdaysContext context)
        {
            _context = context;
        }

        // POST: api/Favorites
        [HttpPost("addFavorite")]
        public async Task<IActionResult> AddFavorite([FromBody] BookingDTO bookingDTO)
        {
            if (bookingDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            // Check if the booking already exists as a favorite
            bool exists = await _context.Bookings.AnyAsync(b =>
                b.UserId == bookingDTO.UserId &&
                b.ActivityId == bookingDTO.ActivityId &&
                b.BookingStatesId == 2);

            if (exists)
            {
                return Conflict("This activity is alre  ady in favorites.");
            }

            var newFavorite = new Booking
            {
                UserId = bookingDTO.UserId,
                ActivityId = bookingDTO.ActivityId,
                BookingDate = bookingDTO.BookingDate,
                Price = bookingDTO.Price,
                BookingStatesId = 2 // Assuming '2' is the ID for 'favorites'
            };

            _context.Bookings.Add(newFavorite);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFavorite), new { userId = bookingDTO.UserId, activityId = bookingDTO.ActivityId }, newFavorite);
        }

        // DELETE: api/Favorites/{userId}/{activityId}
        [HttpDelete("removeFavorite/{userId}/{activityId}")]

        public async Task<IActionResult> RemoveFavorite(int userId, int activityId)
        {
            var favorite = await _context.Bookings.FirstOrDefaultAsync(b =>
                b.UserId == userId &&
                b.ActivityId == activityId &&
                b.BookingStatesId == 2);

            if (favorite == null)
            {
                return NotFound("Favorite not found.");
            }

            _context.Bookings.Remove(favorite);
            await _context.SaveChangesAsync();

            return Ok("Like Removed!");
        }

        // GET: api/Favorites/{userId}
        // This endpoint might be useful to list all favorites for a user
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetFavorites(int userId)
        {
            var favorites = await _context.Bookings
                .Where(b => b.UserId == userId && b.BookingStatesId == 2)
                .Select(b => new
                {
                    ActivityId = b.ActivityId,
                    ActivityName = b.Activity.Name,
                    Price = b.Price
                }).ToListAsync();

            if (!favorites.Any())
            {
                return NotFound("No favorites found.");
            }

            return Ok(favorites);
        }
        // GET: api/Favorites/{userId}/{activityId}
        [HttpGet("{userId}/{activityId}")]
        public async Task<ActionResult<Booking>> GetFavorite(int userId, int activityId)
        {
            var favorite = await _context.Bookings.FirstOrDefaultAsync(b =>
                b.UserId == userId &&
                b.ActivityId == activityId &&
                b.BookingStatesId == 2);

            if (favorite == null)
            {
                return NotFound("Favorite not found.");
            }

            return Ok(favorite);
        }

    }
}
