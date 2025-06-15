﻿using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class ProductGallery
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? ImageName { get; set; }

    public virtual Product? Product { get; set; }
}
