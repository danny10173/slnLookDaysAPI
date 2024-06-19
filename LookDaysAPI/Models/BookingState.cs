using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

public partial class BookingState
{
    [Key]
    [Column("BookingStatesID")]
    public int BookingStatesId { get; set; }

    [StringLength(50)]
    public string? States { get; set; }

    [InverseProperty("BookingStates")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
