using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using LookDaysAPI.Models;

// 設定路由為 "api/Activities"
[Route("api/[controller]")]
[ApiController]
public class HotActivitiesController : ControllerBase
{
    private readonly LookdaysContext _context;

    // 控制器建構函數，注入資料庫上下文
    public HotActivitiesController(LookdaysContext context)
    {
        _context = context;
    }

    // 定義 GET 方法來獲取所有活動
    [HttpGet]
    public IActionResult GetActivities()
    {
        try
        {
            var random = new Random();

            // 記錄進入方法的日誌
            Console.WriteLine("Entering GetActivities method.");

            // 檢查是否有活動資料
            if (!_context.Activities.Any())
            {
                Console.WriteLine("No activities found in the database.");
                return NotFound("No activities found.");
            }

            // 分組活動資料根據 CityId
            var groupedActivities = _context.Activities
                .Include(a => a.ActivitiesAlbums) // 加載相關的 ActivitiesAlbums
                .Include(a => a.City) // 加載與活動相關的城市數據
                .GroupBy(a => a.CityId)
                .ToList();

            // 記錄分組後的活動數量
            Console.WriteLine($"Number of grouped activities: {groupedActivities.Count}");

            // 確認是否有分組後的活動
            if (!groupedActivities.Any())
            {
                Console.WriteLine("No grouped activities found.");
                return NotFound("No grouped activities found.");
            }

            // 從每個分組中隨機選取一個活動
            var selectedFromGroups = groupedActivities
                .SelectMany(g => g.OrderBy(a => random.Next()).Take(1))
                .ToList();

            // 記錄從每個分組中選取的活動數量
            Console.WriteLine($"Number of activities selected from groups: {selectedFromGroups.Count}");

            // 從選取的活動中再隨機選取四個活動
            var selectedActivities = selectedFromGroups
                .OrderBy(a => random.Next())
                .Take(4)
                .Select(a => new
                {
                    a.ActivityId,
                    a.Name,
                    a.Description,
                    a.Price,
                    a.Date,
                    a.CityId,
                    CityName = a.City.CityName, // 包含城市名稱
                    a.Remaining,
                    a.HotelId,
                    // 將照片轉換為 Base64 字串
                    Albums = a.ActivitiesAlbums.Select(album => album.Photo != null ? Convert.ToBase64String(album.Photo) : null).ToList()
                })
                .ToList();

            // 記錄最終選取的活動數量
            Console.WriteLine($"Final number of selected activities: {selectedActivities.Count}");

            // 返回選取的活動資料
            return Ok(selectedActivities);
        }
        catch (Exception ex)
        {
            // 記錄詳細的錯誤訊息
            Console.WriteLine($"Error in GetActivities method: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // 定義 GET 方法來獲取最受歡迎的活動
    [HttpGet("top-booked")]
    public IActionResult GetTopBookedActivities()
    {
        try
        {
            // 獲取最受歡迎的活動資料
              var topBookedActivities = _context.Bookings
             .Include(b => b.Activity) // 加載與預訂相關的活動數據
                .ThenInclude(a=>a.ActivitiesAlbums)
             .Include(b => b.Activity.City) // 加載與活動相關的城市數據
             .GroupBy(b => new { b.Activity.ActivityId, b.Activity.Name, b.Activity.Description, b.Activity.City.CityId, b.Activity.City.CityName })
             .Select(g => new
             {
                 ActivityId = g.Key.ActivityId, // 活動ID
                 Name = g.Key.Name, // 活動名稱
                 Description = g.Key.Description, // 活動描述
                 BookingCount = g.Count(), // 預訂次數，即分組中的記錄數
                 CityId = g.Key.CityId, // 城市ID
                 CityName = g.Key.CityName, // 城市名稱
                 Albums = g.SelectMany(b => b.Activity.ActivitiesAlbums)
                 .Select(album=>album.Photo!=null?Convert.ToBase64String(album.Photo) : null)
                 .ToList()
             })
             .OrderByDescending(a => a.BookingCount) // 根據預訂次數排序
             .Take(10) // 取前10個活動
             .ToList();


            // 返回最受歡迎的活動資料
            return Ok(topBookedActivities);
        }
        catch (Exception ex)
        {
            // 記錄錯誤訊息
            Console.WriteLine($"Error in GetTopBookedActivities method: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpGet("car-activities")]
    public IActionResult GetCarActivities()
    {
        try
        {
            var carActivities = _context.Activities
                .Select(a => new
                {
                    a.ActivityId,
                    a.Name,
                    a.Price,
                    a.Description,
                    a.Date,
                    CityName = a.City.CityName,
                    a.Remaining,
                    a.HotelId,
                    a.Latitude,
                    a.Longitude,
                    Albums = a.ActivitiesAlbums.Select(album => album.Photo != null ? Convert.ToBase64String(album.Photo) : null).ToList()
                })
                .ToList();

            return Ok(carActivities);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetCarActivities method: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
