using System;

namespace ReactApp1.Server.Models
{
    public class BookingHistoryDTO
    {
        public int BookingId { get; set; } // 添加這行
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string ActivityName { get; set; }

        public int BookingStatesId { get; set; }

        public int ActivityId { get; set; }
        public string ActivityDescription { get; set; }
    }
}
