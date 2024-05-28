using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public byte[]? CityPhoto { get; set; }

    public string? Description { get; set; }

    public int CountryId { get; set; }

    public string? CitySide { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual Country Country { get; set; } = null!;
}
