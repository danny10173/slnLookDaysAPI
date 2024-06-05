using LookDaysAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityWithAlbumController : ControllerBase
    {
        private readonly LookdaysContext _context;
        public ActivityWithAlbumController(LookdaysContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetActivities()
        {
            try
            {
                var random = new Random();

                // 記錄進入方法
                Console.WriteLine("Entering GetActivities method.");

                // 檢查是否有活動資料
                if (!_context.Activities.Any())
                {
                    Console.WriteLine("No activities found in the database.");
                    return NotFound("No activities found.");
                }

                var joinedActivities = _context.Activities
                 .Include(a => a.ActivitiesAlbums) // 加載相關的 ActivitiesAlbums
                                                   //.Join(
                                                   //    _context.ActivitiesAlbums, // 第二個序列
                                                   //    a => a.ActivityId,         // 第一個序列的關聯鍵
                                                   //    aa => aa.ActivityId,       // 第二個序列的關聯鍵
                                                   //    (activity, album) => new { Activity = activity, Album = album } // 聯結後的結果
                                                   //)
                 .ToList();


                // 確認是否有分組後的活動
                if (!joinedActivities.Any())
                {
                    Console.WriteLine("No joined activities found.");
                    return NotFound("No joined activities found.");
                }

                var selectedActivities = joinedActivities
                    .Select(a => new
                    {
                        a.ActivityId,
                        a.Name,
                        a.Description,
                        a.Price,
                        a.Date,
                        a.CityId,
                        a.Remaining,
                        a.HotelId,
                        photo = a.ActivitiesAlbums.Select(album => album.Photo != null ? Convert.ToBase64String(album.Photo) : null).ToList()
                    })
                    .ToList();

                // 記錄選取的活動數量
                Console.WriteLine($"Selected {selectedActivities.Count} activities.");

                return Ok(selectedActivities);
            }
            catch (Exception ex)
            {
                // 記錄詳細的錯誤訊息
                Console.WriteLine($"Error in GetActivities method: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
