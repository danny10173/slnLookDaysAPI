using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LookDaysAPI.Models;

[Table("CreditCardInfo")]
public partial class CreditCardInfo
{
    [Key]
    [Column("CInfoID")]
    public int CinfoId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    public int CardNumber { get; set; }

    [Column(TypeName = "date")]
    public DateTime ExpirationDate { get; set; }

    public int CardSecurityCode { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("CreditCardInfos")]
    public virtual User User { get; set; } = null!;
}
