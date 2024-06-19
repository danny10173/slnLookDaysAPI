using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ForumPost")]
public partial class ForumPost
{
    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PostTitle { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PostTime { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? PostContent { get; set; }

    public int? Participants { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ForumPosts")]
    public virtual User? User { get; set; }
}
