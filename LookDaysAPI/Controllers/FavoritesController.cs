using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;

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
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingDTO bookingDTO)
        {
            if (bookingDTO == null)
            {
                return BadRequest();
            }

            // 檢查是否已經存在收藏記錄
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == bookingDTO.UserId && b.ActivityId == bookingDTO.ActivityId && b.BookingStatesId == 2);

            if (existingBooking != null)
            {
                return Conflict("Already in favorites");
            }

            var booking = new Booking
            {
                UserId = 2, //寫死
                ActivityId = bookingDTO.ActivityId,
                BookingDate = bookingDTO.BookingDate,
                Price = bookingDTO.Price,
                BookingStatesId = 2
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBooking([FromQuery] int userId, [FromQuery] int activityId)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ActivityId == activityId && b.BookingStatesId == 2);

            if (booking == null)
            {
                return NotFound("The item is not in favorites.");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/Favorites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Favorites/5

        [HttpGet("favorites")]
        public async Task<ActionResult<IEnumerable<object>>> GetFavorites(int userId)
        {
            var favorites = await _context.Bookings
                .Where(b => b.UserId == userId && b.BookingStatesId == 2)
                .Select(b => new
                {
                    Id = b.ActivityId,
                    Image = _context.ActivitiesAlbums
                                .Where(a => a.ActivityId == b.ActivityId)
                                .Select(a => a.Photo)
                                .FirstOrDefault(),
                    Title = b.Activity.Name,
                    Desc = b.Activity.Description,
                    Price = b.Price
                })
                .ToListAsync();

            if (favorites == null || favorites.Count == 0)
            {
                return NotFound("No favorites found.");
            }

            return Ok(favorites);
        }

        // PUT: api/Favorites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Favorites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Bookings
        

        // DELETE: api/Favorites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
    }
}
