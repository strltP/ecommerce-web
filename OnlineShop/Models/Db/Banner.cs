using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.Db;

public partial class Banner
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters long.")]
    public string? Title { get; set; }
    [Required]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters long.")]
    public string? SubTitle { get; set; }

    public string? ImageName { get; set; }
    [Required,Range(1,10)]
    public short? Priority { get; set; }

    public string? Link { get; set; }
    [Required]
    public string? Position { get; set; }
}
