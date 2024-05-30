namespace LookDaysAPI.Models.DTO
{
    public class ActivityDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
        public int CityId { get; set; }
        public int? Remaining { get; set; }
        public int? HotelId { get; set; }
    }
}
