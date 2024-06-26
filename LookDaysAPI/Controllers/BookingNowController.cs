using LookDaysAPI.Models;
using LookDaysAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace LookDaysAPI.Controllers
{

    // 設置控制器的路由和API控制器屬性
    [Route("api/[controller]")]
    [ApiController]

    public class BookingNowController : Controller
    {
        private readonly LookdaysContext _context; // 定義數據庫上下文
        public BookingNowController(LookdaysContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // 注入數據庫上下文並檢查空引用
        }

        // 添加立即預訂清單
        [HttpPost]
        public IActionResult AddBookingNow(int? UserId, int? ActivityId, int? ModelId, int? Quantity)
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
                    UserId = UserId ?? 0, // 確保非 null 值
                    ActivityId = ActivityId ?? 0, // 確保非 null 值
                    ModelId = ModelId, // 這裡可能為 null
                    BookingDate = DateTime.Now,
                    Price = (decimal)(productPrice * Quantity), // 確保非 null 值
                    BookingStatesId = 4, // 設置為立即預定狀態
                    Member = Quantity ?? 0 // 確保非 null 值
                };

                // 將新預訂添加到數據庫並保存
                _context.Bookings.Add(newBooking);
                _context.SaveChanges();

                // 查找剛剛創建的預訂記錄，並包括必要的聯接
                var recentBooking = (from r in _context.Bookings
                                     where r.UserId == UserId && r.BookingStatesId == 4
                                     orderby r.BookingDate descending
                                     select new BookingNowDto
                                     {
                                         BookingId = r.BookingId,
                                         ActivityName = r.Activity.Name,
                                         ActivityDate = r.Activity.Date ?? DateTime.MinValue, // 確保非 null 值
                                         ActivityPhoto = _context.ActivitiesAlbums
                                             .Where(a => a.ActivityId == r.ActivityId)
                                             .Select(a => Convert.ToBase64String(a.Photo))
                                             .FirstOrDefault(),
                                         ModelName = r.ModelId.HasValue ? _context.ActivitiesModels
                                             .Where(m => m.ModelId == r.ModelId)
                                             .Select(m => m.ModelName)
                                             .FirstOrDefault() : null,
                                         Price = r.Price,
                                         Member = r.Member,
                                         ModelId = r.ModelId ?? 0, // 確保非 null 值
                                         ModelPrice = r.ModelId.HasValue ? _context.ActivitiesModels
                                             .Where(m => m.ModelId == r.ModelId)
                                             .Select(m => m.ModelPrice)
                                             .FirstOrDefault() ?? 0m : 0m // 確保非 null 值
                                     }).FirstOrDefault();

                Console.WriteLine($"Booking ID: {recentBooking.BookingId}, Price: {recentBooking.Price}");
                // 打印返回的數據結構
                Console.WriteLine($"API Response: {Newtonsoft.Json.JsonConvert.SerializeObject(recentBooking)}");

                return Json(recentBooking);
                //var recentBooking = (from r in _context.Bookings
                //                     where r.UserId == UserId && r.BookingStatesId == 4
                //                     orderby r.BookingDate descending
                //                     select new BookingNowDto
                //                     {
                //                         BookingId = r.BookingId,
                //                         ActivityName = r.Activity.Name,
                //                         ActivityDate = r.Activity.Date,
                //                         ActivityPhoto = _context.ActivitiesAlbums
                //                             .Where(a => a.ActivityId == r.ActivityId)
                //                             .Select(a => Convert.ToBase64String(a.Photo))
                //                             .FirstOrDefault(),
                //                         ModelName = r.ModelId.HasValue ? _context.ActivitiesModels
                //                             .Where(m => m.ModelId == r.ModelId)
                //                             .Select(m => m.ModelName)
                //                             .FirstOrDefault() : null,
                //                         Price = r.Price,
                //                         Member = r.Member
                //                     }).FirstOrDefault();

                //Console.WriteLine($"Booking ID: {recentBooking.BookingId}, Price: {recentBooking.Price}");
                //// 打印返回的數據結構
                //Console.WriteLine($"API Response: {Newtonsoft.Json.JsonConvert.SerializeObject(recentBooking)}");

                //return Json(recentBooking);
            }
            catch (Exception ex)
            {
                // 如果發生異常，返回內部伺服器錯誤
                return StatusCode(500, "內部伺服器錯誤");
            }
        }



    }
}
