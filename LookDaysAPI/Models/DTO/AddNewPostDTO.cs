namespace LookDaysAPI.Models.DTO
{
    public partial class AddNewPostDTO
    {
        public string? PostTitle { get; set; }

        public int? UserId { get; set; }

        public DateTime? PostTime { get; set; }

        public string? PostContent { get; set; }

        public int? Participants { get; set; }
    }
}
