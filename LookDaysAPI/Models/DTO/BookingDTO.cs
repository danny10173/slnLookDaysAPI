namespace LookDaysAPI.Models.DTO
{
    public class BookingDTO
    {
        public int UserId { get; set; }

        public int ActivityId { get; set; }

        public DateTime? BookingDate { get; set; }

        public decimal Price { get; set; }

        public int BookingStatesId { get; set; }

        public int? Member { get; set; }
    }
}
