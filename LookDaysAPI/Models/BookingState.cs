using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class BookingState
{
    public int BookingStatesId { get; set; }

    public string? States { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
