using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ActivitiesAlbum")]
public partial class ActivitiesAlbum
{
    [Key]
    [Column("PhotoID")]
    public int PhotoId { get; set; }

    [Column(TypeName = "image")]
    public byte[]? Photo { get; set; }

    [Column("ActivityID")]
    public int ActivityId { get; set; }

    [Column(TypeName = "string")]
    public string? PhotoDesc { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("ActivitiesAlbums")]
    public virtual Activity Activity { get; set; } = null!;
}
