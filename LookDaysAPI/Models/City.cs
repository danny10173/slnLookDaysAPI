using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("City")]
public partial class City
{
    [Key]
    [Column("CityID")]
    public int CityId { get; set; }

    [StringLength(50)]
    public string CityName { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[]? CityPhoto { get; set; }

    public string? Description { get; set; }

    [Column("CountryID")]
    public int CountryId { get; set; }

    [StringLength(50)]
    public string? CitySide { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    [ForeignKey("CountryId")]
    [InverseProperty("Cities")]
    public virtual Country Country { get; set; } = null!;
}
