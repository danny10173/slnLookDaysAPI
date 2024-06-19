using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ModelTagJoint")]
public partial class ModelTagJoint
{
    [Key]
    [Column("TagJointID")]
    public int TagJointId { get; set; }

    [Column("ModelTagID")]
    public int ModelTagId { get; set; }

    [Column("ModelID")]
    public int ModelId { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("ModelTagJoints")]
    public virtual ActivitiesModel Model { get; set; } = null!;

    [ForeignKey("ModelTagId")]
    [InverseProperty("ModelTagJoints")]
    public virtual ModelTag ModelTag { get; set; } = null!;
}
