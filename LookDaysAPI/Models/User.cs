using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Preferences { get; set; }

    public int RoleId { get; set; }

    public string? UserPic { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
}
