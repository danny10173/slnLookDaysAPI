using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class ActionJoint
{
    public int ActionJointId { get; set; }

    public int UserId { get; set; }

    public int ActivityId { get; set; }

    public int ActionTypeId { get; set; }

    public virtual ActionType ActionType { get; set; } = null!;

    public virtual Activity Activity { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
