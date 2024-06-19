using Newtonsoft.Json;

namespace LookDaysAPI.Models;

public partial class ForumPost
{
    public int PostId { get; set; }

    public int? UserId { get; set; }

    public string? PostTitle { get; set; }

    public DateTime? PostTime { get; set; }

    public string? PostContent { get; set; }

    public int Participants { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
