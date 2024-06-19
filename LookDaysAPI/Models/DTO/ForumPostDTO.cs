namespace LookDaysAPI.Models.DTO
{
    public class ForumPostDTO
    {
        public int PostId { get; set; }

        public int? UserId { get; set; }

        public string Username { get; set; } = null!;

        public string? PostTitle { get; set; }

        public DateTime? PostTime { get; set; }

        public string? PostContent { get; set; }

        public int? Participants { get; set; }
    }
}
