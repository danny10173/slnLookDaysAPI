using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class ClassJoint
{
    public int ClassJointId { get; set; }

    public int ActivityId { get; set; }

    public int ClassId { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual ClassName Class { get; set; } = null!;
}
