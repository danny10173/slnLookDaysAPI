using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("ActivitiesModel")]
public partial class ActivitiesModel
{
    [Key]
    [Column("ModelID")]
    public int ModelId { get; set; }

    [Column("ActivityID")]
    public int? ActivityId { get; set; }

    [StringLength(50)]
    public string? ModelName { get; set; }

    [Column(TypeName = "money")]
    public decimal? ModelPrice { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ModelDate { get; set; }

    [StringLength(50)]
    public string? ModelContent { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("ActivitiesModels")]
    public virtual Activity? Activity { get; set; }

    [InverseProperty("Model")]
    public virtual ICollection<ModelTagJoint> ModelTagJoints { get; set; } = new List<ModelTagJoint>();
}
