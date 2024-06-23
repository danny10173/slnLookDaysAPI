using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

public partial class Activity
{
    [Key]
    [Column("ActivityID")]
    public int ActivityId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal? Price { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Date { get; set; }

    [Column("CityID")]
    public int? CityId { get; set; }

    public int? Remaining { get; set; }

    [Column("HotelID")]
    public int? HotelId { get; set; }

    [Column("latitude")]
    public double? Latitude { get; set; }

    [Column("longitude")]
    public double? Longitude { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    public string? DescriptionJson { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<ActionJoint> ActionJoints { get; set; } = new List<ActionJoint>();

    [InverseProperty("Activity")]
    public virtual ICollection<ActivitiesAlbum> ActivitiesAlbums { get; set; } = new List<ActivitiesAlbum>();

    [InverseProperty("Activity")]
    public virtual ICollection<ActivitiesModel> ActivitiesModels { get; set; } = new List<ActivitiesModel>();

    [InverseProperty("Activity")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Activity")]
    public virtual ICollection<BrowsingHistory> BrowsingHistories { get; set; } = new List<BrowsingHistory>();

    [ForeignKey("CityId")]
    [InverseProperty("Activities")]
    public virtual City? City { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<ClassJoint> ClassJoints { get; set; } = new List<ClassJoint>();

    [ForeignKey("HotelId")]
    [InverseProperty("Activities")]
    public virtual Hotel? Hotel { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public string? DescriptionJson { get; set; } // JSON formatted descriptions
}
