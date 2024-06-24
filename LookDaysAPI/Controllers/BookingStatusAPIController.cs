using LookDaysAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Linq;
using System.Threading.Tasks;

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
            // 獲取所有 MerchantTradeNo 不為空且 BookingStatesId 等於 1 的訂單
            var bookings = await _context.Bookings
                .Where(b => b.MerchantTradeNo != null && b.BookingStatesId == 1)
                .ToListAsync();

            // 更新訂單狀態
            foreach (var booking in bookings)
            {
                booking.BookingStatesId = 3;
            }

            // 保存更改
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}