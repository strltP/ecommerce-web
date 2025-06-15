using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? Password { get; set; }

    public bool? IsAdmin { get; set; }

    public DateTime? RegisterDate { get; set; }

    public int? RecoveryCode { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
