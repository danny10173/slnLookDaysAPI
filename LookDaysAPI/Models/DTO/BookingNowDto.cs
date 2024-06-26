namespace LookDaysAPI.Models.DTO
{
    public class BookingNowDto
    {
        public int BookingId { get; set; }
        public string ActivityName { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityPhoto { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public int? Member { get; set; }
        public int ModelId { get; set; }
        public decimal ModelPrice { get; set; }  // 添加這個字段
    }
}
