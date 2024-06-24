using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LookDaysAPI.Models.Service;
using Microsoft.EntityFrameworkCore;
using LookDaysAPI.Models;


namespace ReactApp1.Server.Controllers
{
    // 設置控制器的路由和API控制器屬性
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartApiController : Controller
    {
        private readonly LookdaysContext _context; // 定義數據庫上下文
        public ShoppingCartApiController(LookdaysContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // 注入數據庫上下文並檢查空引用
        }

        // 添加購物車項目
        [HttpPost]
        public IActionResult AddShoppingCart(int? UserId, int? ActivityId)
        {
            // 檢查UserId和ActivityId是否為空
            if (UserId == null)
                return BadRequest("請登入會員");
            if (ActivityId == null)
                return BadRequest("DB沒此活動ID");

            try
            {
                // 查詢活動價格
                var productPrices = (from r in _context.Activities
                                     where r.ActivityId == ActivityId
                                     select r.Price).FirstOrDefault();

                // 如果沒有找到價格，返回未找到錯誤
                if (productPrices == null)
                {
                    return NotFound("未找到商品價格");
                }

                // 創建新的預訂記錄
                Booking newBooking = new Booking()
                {
                    UserId = (int)UserId,
                    ActivityId = (int)ActivityId,
                    BookingDate = DateTime.Now,
                    Price = Convert.ToDecimal(productPrices),
                    BookingStatesId = 1, // 設置為購物車狀態
                    Member = 1
                };

                // 將新預訂添加到數據庫並保存
                _context.Bookings.Add(newBooking);
                _context.SaveChanges();

                return Ok("success");
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回內部服務器錯誤
                return StatusCode(500, "內部伺服器錯誤");
            }
        }

        // 獲取用戶購物車的所有行程
        [HttpPost("get")]
        public async Task<ActionResult<IEnumerable<object>>> GetCartItems([FromBody] int userID)
        {
            // 查詢用戶購物車中的所有項目，並包括活動的名稱、日期和照片
            var cartItems = await _context.Bookings
                                          .Where(b => b.UserId == userID && b.BookingStatesId == 1)
                                          .Select(b => new
                                          {
                                              b.BookingId,
                                              b.UserId,
                                              b.ActivityId,
                                              b.BookingDate,
                                              b.Price,
                                              b.BookingStatesId,
                                              b.Member,
                                              Activity = _context.Activities
                                                                .Where(a => a.ActivityId == b.ActivityId)
                                                                .Select(a => new
                                                                {
                                                                    a.Name,
                                                                    a.Date,
                                                                    Photo = _context.ActivitiesAlbums
                                                                                    .Where(al => al.ActivityId == a.ActivityId)
                                                                                    .Select(al => al.Photo)
                                                                                    .FirstOrDefault()
                                                                })
                                                                .FirstOrDefault()
                                          })
                                          .ToListAsync();

            return Ok(cartItems); // 返回購物車項目
        }

        // 更新購物車中的行程數量
        [HttpPost("update")]
        public async Task<IActionResult> UpdateCart([FromBody] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest("無效的資料");
            }

            // 查詢現有的預訂項目
            var existingBooking = await _context.Bookings.FindAsync(booking.BookingId);
            if (existingBooking == null)
            {
                return NotFound("未找到購物車項目");
            }

            // 更新人數並保存更改
            existingBooking.Member = booking.Member;
            _context.Entry(existingBooking).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "購物車更新成功" });
        }

        // 刪除購物車中的行程
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteCartItem([FromBody] int bookingID)
        {
            // 查詢購物車項目
            var cartItem = await _context.Bookings.FindAsync(bookingID);
            if (cartItem == null)
            {
                return NotFound("未找到購物車項目");
            }

            // 刪除購物車項目並保存更改
            _context.Bookings.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "購物車項目已刪除" });
        }
    }
}
