namespace LookDaysAPI.Models.DTO
{
    public class ChatMessageDto
    {
        public int ChatMessageId { get; set; }
        public int UserId { get; set; }
        public string ChatRoom { get; set; }
        public string Username { get; set; }
        public string ChatContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
