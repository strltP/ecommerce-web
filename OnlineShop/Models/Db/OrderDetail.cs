using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class OrderDetail
{
    public int Id { get; set; }

    public string? ProductTitle { get; set; }

    public decimal? ProductPrice { get; set; }

    public int? Count { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
