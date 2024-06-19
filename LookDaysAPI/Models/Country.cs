using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("Country")]
public partial class Country
{
    [Key]
    [Column("CountryID")]
    public int CountryId { get; set; }

    [Column("Country")]
    [StringLength(50)]
    public string? Country1 { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
