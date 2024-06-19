using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ActionJoint")]
public partial class ActionJoint
{
    [Key]
    [Column("ActionJointID")]
    public int ActionJointId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("ActivityID")]
    public int ActivityId { get; set; }

    [Column("ActionTypeID")]
    public int ActionTypeId { get; set; }

    [ForeignKey("ActionTypeId")]
    [InverseProperty("ActionJoints")]
    public virtual ActionType ActionType { get; set; } = null!;

    [ForeignKey("ActivityId")]
    [InverseProperty("ActionJoints")]
    public virtual Activity Activity { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ActionJoints")]
    public virtual User User { get; set; } = null!;
}
