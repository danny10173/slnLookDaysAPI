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

    public virtual ICollection<ActionJoint> ActionJoints { get; set; } = new List<ActionJoint>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CreditCardInfo> CreditCardInfos { get; set; } = new List<CreditCardInfo>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;
}
