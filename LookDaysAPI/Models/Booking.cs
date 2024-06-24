using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

public partial class Booking
{
    [Key]
    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("ActivityID")]
    public int ActivityId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BookingDate { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Column("BookingStatesID")]
    public int BookingStatesId { get; set; }

    public int? Member { get; set; }

    [Column("ModelID")]
    public int? ModelId { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("Bookings")]
    public virtual Activity Activity { get; set; } = null!;

    [ForeignKey("BookingStatesId")]
    [InverseProperty("Bookings")]
    public virtual BookingState BookingStates { get; set; } = null!;

    [InverseProperty("Booking")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual User User { get; set; } = null!;

    public string? MerchantTradeNo { get; set; }
}
