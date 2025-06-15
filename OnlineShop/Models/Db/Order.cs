using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string? Country { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Comment { get; set; }

    public string? CouponCode { get; set; }

    public decimal? CouponDiscount { get; set; }

    public decimal? Shipping { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? Total { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? TransId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
