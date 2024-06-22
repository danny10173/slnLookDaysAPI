using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("BrowsingHistory")]
public partial class BrowsingHistory
{
    [Key]
    [Column("BrowsingHistoryID")]
    public int BrowsingHistoryId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("ActivityID")]
    public int? ActivityId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BrowseTime { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("BrowsingHistories")]
    public virtual Activity? Activity { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("BrowsingHistories")]
    public virtual User? User { get; set; }
}
