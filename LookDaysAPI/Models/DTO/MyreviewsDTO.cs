using LookDaysAPI.Models;

namespace ReactApp1.Server.Models.DTO
{
    public class MyReviewsDTO
    {
        public int UserId { get; set; }
        public string ActivityName { get; set; }
        public int ActivityId { get; set; }
        public string Comment { get; set; }
        public double? Rating { get; set; }
        public virtual ICollection<ActivitiesAlbum> ActivitiesAlbums { get; set; } = new List<ActivitiesAlbum>();
    }
}
