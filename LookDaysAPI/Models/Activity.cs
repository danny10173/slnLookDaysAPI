﻿using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class Activity
{
    public int ActivityId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime? Date { get; set; }

    public int CityId { get; set; }

    public int? Remaining { get; set; }

    public int? HotelId { get; set; }

    public virtual ICollection<ActionJoint> ActionJoints { get; set; } = new List<ActionJoint>();

    public virtual ICollection<ActivitiesAlbum> ActivitiesAlbums { get; set; } = new List<ActivitiesAlbum>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<ClassJoint> ClassJoints { get; set; } = new List<ClassJoint>();

    public virtual Hotel? Hotel { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}