using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ClassJoint")]
public partial class ClassJoint
{
    [Key]
    [Column("ClassJointID")]
    public int ClassJointId { get; set; }

    [Column("ActivityID")]
    public int ActivityId { get; set; }

    [Column("ClassID")]
    public int ClassId { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("ClassJoints")]
    public virtual Activity Activity { get; set; } = null!;

    [ForeignKey("ClassId")]
    [InverseProperty("ClassJoints")]
    public virtual ClassName Class { get; set; } = null!;
}
