using LookDaysAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingHistoryController : ControllerBase
    {
        private readonly LookdaysContext _context;

        public BookingHistoryController(LookdaysContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingHistoryDTO>>> GetBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.BookingStates)
                .Include(b => b.User)
                .Include(b => b.Payments)
                .ToListAsync();

            var bookingDTOs = bookings.Select(b => new BookingHistoryDTO
            {
                BookingId = b.BookingId, // 添加這行
                UserId = b.UserId,
                Price = b.Price,
                ActivityName = b.Activity.Name,
                ActivityDescription = b.Activity.Description
            }).ToList();

            return Ok(bookingDTOs);
        }

        // GET: api/Bookings/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<BookingHistoryDTO>>> GetBookingsByUserId(int userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Activity)
                .Include(b => b.BookingStates)
                .Include(b => b.User)
                .Include(b => b.Payments)
                .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound();
            }

            var bookingHisDTOs = bookings.Select(b => new BookingHistoryDTO
            {
                BookingId = b.BookingId, // 添加這行
                UserId = b.UserId,
                Price = b.Price,
                ActivityName = b.Activity.Name,
                ActivityDescription = b.Activity.Description
            }).ToList();

            return Ok(bookingHisDTOs);
        }

    }
}
