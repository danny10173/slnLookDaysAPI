using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

public partial class Review
{
    [Key]
    [Column("ReviewID")]
    public int ReviewId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("ActivityID")]
    public int ActivityId { get; set; }

    [StringLength(500)]
    public string Comment { get; set; } = null!;

    public double? Rating { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("Reviews")]
    public virtual Activity Activity { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Reviews")]
    public virtual User User { get; set; } = null!;
}
