using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

public partial class Hotel
{
    [Key]
    [Column("HotelID")]
    public int HotelId { get; set; }

    [StringLength(60)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Address { get; set; }

    [Column(TypeName = "image")]
    public byte[]? HotelPhoto { get; set; }

    [InverseProperty("Hotel")]
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
