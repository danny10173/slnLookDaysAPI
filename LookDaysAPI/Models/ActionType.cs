using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ActionType")]
public partial class ActionType
{
    [Key]
    [Column("ActionTypeID")]
    public int ActionTypeId { get; set; }

    [StringLength(50)]
    public string? Action { get; set; }

    [InverseProperty("ActionType")]
    public virtual ICollection<ActionJoint> ActionJoints { get; set; } = new List<ActionJoint>();
}
