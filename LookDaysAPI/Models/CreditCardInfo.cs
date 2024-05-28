using System;
using System.Collections.Generic;

namespace LookDaysAPI.Models;

public partial class CreditCardInfo
{
    public int CinfoId { get; set; }

    public int UserId { get; set; }

    public int CardNumber { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int CardSecurityCode { get; set; }

    public virtual User User { get; set; } = null!;
}
