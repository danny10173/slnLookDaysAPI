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
    public class BookingsController : ControllerBase
    {
        private readonly LookdaysContext _context;

        public BookingsController(LookdaysContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
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

            return booking;
        }

        // PUT: api/Bookings/5
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

        [HttpDelete]
        public IActionResult DeleteBooking([FromBody] BookingDTO bookingDTO)
        {
            // 檢查 bookingDTO 是否為 null，如果是，返回 400 BadRequest 響應
            if (bookingDTO == null)
            {
                return BadRequest();
            }

            // 查找相應的 Booking 記錄
            var booking = _context.Bookings
                .FirstOrDefault(b => b.UserId == bookingDTO.UserId && b.ActivityId == bookingDTO.ActivityId && b.BookingStatesId == 2);

            // 如果找不到該記錄，返回 404 NotFound 響應
            if (booking == null)
            {
                return NotFound("The item is not in favorites.");
            }

            // 從數據庫中刪除該記錄
            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            // 返回 200 OK 響應，表示操作成功
            return Ok();
        }



        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Bookings
        [HttpPost]
        public IActionResult AddBooking([FromBody] BookingDTO bookingDTO)
        {
            // 檢查 bookingDTO 是否為 null，如果是，返回 400 BadRequest 響應
            if (bookingDTO == null)
            {
                return BadRequest();
            }

            var booking = new Booking
            {
                BookingStatesId = 2,
                ActivityId = bookingDTO.ActivityId,
                Price = bookingDTO.Price,
                Member = bookingDTO.Member,
                UserId = 2, //寫死
                BookingDate = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/Bookings/5
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
