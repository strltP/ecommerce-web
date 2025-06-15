using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class Comment
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? CommentText { get; set; }

    public int? ProductId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Product? Product { get; set; }
}
