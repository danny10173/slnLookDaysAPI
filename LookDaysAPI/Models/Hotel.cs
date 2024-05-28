using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class Hotel
{
    public int HotelId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Address { get; set; }

    public byte[]? HotelPhoto { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
