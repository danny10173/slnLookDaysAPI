using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class ClassName
{
    public int ClassId { get; set; }

    public string? ClassName1 { get; set; }

    public virtual ICollection<ClassJoint> ClassJoints { get; set; } = new List<ClassJoint>();
}
