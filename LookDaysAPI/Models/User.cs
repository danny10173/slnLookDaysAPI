using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("User")]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(24)]
    public string Username { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(64)]
    public string Password { get; set; } = null!;

    public int? Preferences { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    [Column(TypeName = "image")]
    public byte[]? UserPic { get; set; }

    [Column("fPhone")]
    [StringLength(50)]
    public string? fPhone { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<ActionJoint> ActionJoints { get; set; } = new List<ActionJoint>();

    [InverseProperty("User")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("User")]
    public virtual ICollection<BrowsingHistory> BrowsingHistories { get; set; } = new List<BrowsingHistory>();

    [InverseProperty("User")]
    public virtual ICollection<CreditCardInfo> CreditCardInfos { get; set; } = new List<CreditCardInfo>();

    [InverseProperty("User")]
    public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;
}
