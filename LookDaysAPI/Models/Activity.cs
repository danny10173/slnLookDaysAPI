using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class Activity
{
    public int ActivityId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public DateTime? Date { get; set; }

    public int? CityId { get; set; }

    public int? Remaining { get; set; }

    public int? HotelId { get; set; }

    public virtual ICollection<ActivitiesModel> ActivitiesModels { get; set; } = new List<ActivitiesModel>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
