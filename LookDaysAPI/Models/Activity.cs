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

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public virtual ICollection<ActionJoint> ActionJoints { get; set; } = new List<ActionJoint>();

    public virtual ICollection<ActivitiesAlbum> ActivitiesAlbums { get; set; } = new List<ActivitiesAlbum>();

    public virtual ICollection<ActivitiesModel> ActivitiesModels { get; set; } = new List<ActivitiesModel>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual City? City { get; set; }

    public virtual ICollection<ClassJoint> ClassJoints { get; set; } = new List<ClassJoint>();

    public virtual Hotel? Hotel { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
