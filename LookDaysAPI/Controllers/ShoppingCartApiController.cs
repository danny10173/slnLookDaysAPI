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
        public IActionResult AddShoppingCart(int? UserId, int? ActivityId, int? ModelId, int? Quantity)
        {
            // 檢查 UserId 和 ActivityId 是否為空
            if (UserId == null)
                return BadRequest("請登入會員");
            if (ActivityId == null)
                return BadRequest("DB沒此活動ID");
            if (Quantity == null || Quantity <= 0)
                return BadRequest("請選擇有效的人數");
            try
            {
                decimal? productPrice = null;

                if (ModelId != null)
                {
                    // 如果提供了 ModelId，查詢該 Model 的價格
                    productPrice = (from r in _context.ActivitiesModels
                                    where r.ActivityId == ActivityId && r.ModelId == ModelId
                                    select r.ModelPrice).FirstOrDefault();

                    // 如果沒有找到價格，返回未找到錯誤
                    if (productPrice == null)
                    {
                        return NotFound("未找到商品價格或模式ID無效");
                    }
                }
                else
                {
                    // 如果沒有提供 ModelId，查詢活動的價格
                    productPrice = (from r in _context.Activities
                                    where r.ActivityId == ActivityId
                                    select r.Price).FirstOrDefault();

                    // 如果沒有找到價格，返回未找到錯誤
                    if (productPrice == null)
                    {
                        return NotFound("未找到商品價格");
                    }
                }

                // 創建新的預訂記錄
                Booking newBooking = new Booking()
                {
                    UserId = (int)UserId,
                    ActivityId = (int)ActivityId,
                    ModelId = ModelId, // 這裡可能為 null
                    BookingDate = DateTime.Now,
                    Price = Convert.ToDecimal(productPrice) * (int)Quantity,
                    BookingStatesId = 1, // 設置為購物車狀態
                    Member = (int)Quantity
                };

                // 將新預訂添加到數據庫並保存
                _context.Bookings.Add(newBooking);
                _context.SaveChanges();

                return Ok("success");
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回內部伺服器錯誤
                return StatusCode(500, "內部伺服器錯誤");
            }
        }

        // 獲取用戶購物車的所有行程
        [HttpPost("get")]
        public async Task<ActionResult<IEnumerable<object>>> GetCartItems([FromBody] int userID)
        {
            // 查詢用戶購物車中的所有項目，並包括活動的名稱、日期和照片
            var cartItems = await _context.Bookings
                                          // 過濾條件：匹配給定用戶ID且預訂狀態為1（購物車狀態）的記錄
                                          .Where(b => b.UserId == userID && b.BookingStatesId == 1)
                                          // 選擇所需的字段來構建匿名對象
                                          .Select(b => new
                                          {
                                              // 預訂ID
                                              b.BookingId,
                                              // 用戶ID
                                              b.UserId,
                                              // 活動ID
                                              b.ActivityId,
                                              // 模式ID（可能為null）
                                              b.ModelId,
                                              // 預訂日期
                                              b.BookingDate,
                                              // 價格
                                              b.Price,
                                              // 預訂狀態ID
                                              b.BookingStatesId,
                                              // 人數
                                              b.Member,
                                              // 查詢活動詳細信息，包括名稱、日期和照片
                                              Activity = _context.Activities
                                                                .Where(a => a.ActivityId == b.ActivityId)
                                                                .Select(a => new
                                                                {
                                                                    // 活動名稱
                                                                    a.Name,
                                                                    // 活動日期
                                                                    a.Date,
                                                                    // 查詢活動的第一張照片
                                                                    Photo = _context.ActivitiesAlbums
                                                                                    .Where(al => al.ActivityId == a.ActivityId)
                                                                                    .Select(al => al.Photo)
                                                                                    .FirstOrDefault()
                                                                })
                                                                .FirstOrDefault(),
                                              // 如果ModelId有值，查詢模型詳細信息
                                              Model = b.ModelId.HasValue ? _context.ActivitiesModels
                                                                           .Where(m => m.ActivityId == b.ActivityId && m.ModelId == b.ModelId)
                                                                           .Select(m => new
                                                                           {
                                                                               // 模型名稱
                                                                               m.ModelName,
                                                                               // 模型價格
                                                                               m.ModelPrice
                                                                           })
                                                                           .FirstOrDefault() : null
                                          })
                                          .ToListAsync(); // 將結果轉換為列表異步執行

            // 返回購物車項目
            return Ok(cartItems);
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