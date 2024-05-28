using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class ActivitiesAlbum
{
    public int PhotoId { get; set; }

    public byte[]? Photo { get; set; }

    public int ActivityId { get; set; }

    public virtual Activity Activity { get; set; } = null!;
}
