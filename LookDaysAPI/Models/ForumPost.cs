using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class ForumPost
{
    public int PostId { get; set; }

    public int? UserId { get; set; }

    public byte[]? PostTitle { get; set; }

    public DateTime? PostTime { get; set; }

    public string? PostContent { get; set; }

    public int Participants { get; set; }

    public virtual User? User { get; set; }
}
