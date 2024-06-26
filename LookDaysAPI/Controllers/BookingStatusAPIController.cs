using LookDaysAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Linq;
using System.Threading.Tasks;


//UseIDToMerchantTradeNoController 暫時替代此方法

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingStatusAPIController : ControllerBase
    {
        private readonly LookdaysContext _context;

        public BookingStatusAPIController(LookdaysContext context)
        {
            _context = context;
        }

        [HttpPost("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus()
        {
            // 獲取最近的BookingDate
            var latestBooking = await _context.Bookings
                .OrderByDescending(b => b.BookingDate)
                .FirstOrDefaultAsync();

            if (latestBooking == null)
            {
                return NotFound("No bookings found.");
            }

            // 獲取UserId
            var userId = latestBooking.UserId;

            // 獲取當前UserID的MerchantTradeNo不為空且BookingStatesId等於1的訂單
            var bookingsWithStatus1 = await _context.Bookings
                .Where(b => b.MerchantTradeNo != null && b.BookingStatesId == 1 && b.UserId == userId)
                .ToListAsync();


            // 獲取當前UserID所有MerchantTradeNo不為空且BookingStatesId等於4且BookingDate為最近的那一筆資料
            var latestBookingWithStatus4 = await _context.Bookings
                .Where(b => b.MerchantTradeNo != null && b.BookingStatesId == 4 && b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .FirstOrDefaultAsync();

            // 將符合條件的訂單的狀態改為3
            foreach (var booking in bookingsWithStatus1)
            {
                booking.BookingStatesId = 3;
            }
            if (latestBookingWithStatus4 != null)
            {
                latestBookingWithStatus4.BookingStatesId = 3;
            }

            // 保存更改
            await _context.SaveChangesAsync();

            return Ok();
        }



    }
}