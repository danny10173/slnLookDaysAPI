using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

public partial class ModelTag
{
    [Key]
    [Column("ModelTagID")]
    public int ModelTagId { get; set; }

    [StringLength(50)]
    public string? Tags { get; set; }

    [InverseProperty("ModelTag")]
    public virtual ICollection<ModelTagJoint> ModelTagJoints { get; set; } = new List<ModelTagJoint>();
}
