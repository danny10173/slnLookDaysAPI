using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ClassName")]
public partial class ClassName
{
    [Key]
    [Column("ClassID")]
    public int ClassId { get; set; }

    [Column("ClassName")]
    [StringLength(50)]
    public string? ClassName1 { get; set; }

    [InverseProperty("Class")]
    public virtual ICollection<ClassJoint> ClassJoints { get; set; } = new List<ClassJoint>();
}
