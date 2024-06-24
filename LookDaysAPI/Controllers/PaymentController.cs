using LookDaysAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Threading.Tasks;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly LookdaysContext _context;
        private readonly ECPayService _ecPayService;

        public PaymentController(LookdaysContext context, ECPayService ecPayService)
        {
            _context = context;
            _ecPayService = ecPayService;
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] BookingIdsRequest request)
        {
            if (request == null || request.BookingIds == null || !request.BookingIds.Any())
            {
                return BadRequest("BookingIds field is required.");
            }

            var bookings = await _context.Bookings
                .Include(b => b.Activity)
                .Where(b => request.BookingIds.Contains(b.BookingId))
                .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound();
            }

            var totalAmount = bookings.Sum(b => b.Price * (b.Member ?? 1));

            foreach (var booking in bookings)
            {
                booking.MerchantTradeNo = Guid.NewGuid().ToString("N").Substring(0, 20);
            }

            await _context.SaveChangesAsync();

            var paymentForm = _ecPayService.CreatePaymentRequest(bookings, totalAmount);

            return Content(paymentForm, "text/html");
        }

        public class BookingIdsRequest
        {
            public List<int> BookingIds { get; set; }
        }

        [HttpPost("Return")]
        public async Task<IActionResult> Return([FromForm] IFormCollection collection)
        {
            try
            {
                Console.WriteLine("Received Return request");

                foreach (var key in collection.Keys)
                {
                    Console.WriteLine($"{key}: {collection[key]}");
                }

                var checkMacValue = collection["CheckMacValue"];
                var parameters = collection
                    .Where(x => x.Key != "CheckMacValue")
                    .ToDictionary(x => x.Key, x => x.Value.ToString());

                var validCheckMacValue = _ecPayService.GenerateCheckMacValue(parameters);

                Console.WriteLine($"Received CheckMacValue: {checkMacValue}");
                Console.WriteLine($"Generated CheckMacValue: {validCheckMacValue}");

                if (checkMacValue != validCheckMacValue)
                {
                    Console.WriteLine("Invalid CheckMacValue");
                    return BadRequest("Invalid CheckMacValue");
                }

                var merchantTradeNo = collection["MerchantTradeNo"];
                var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.MerchantTradeNo == merchantTradeNo);

                if (booking == null)
                {
                    return NotFound();
                }

                booking.BookingStatesId = 3;
                await _context.SaveChangesAsync();

                // 回應綠界以確認交易結果已被接收和處理
                return Content("1|OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Return method: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        
    }
}