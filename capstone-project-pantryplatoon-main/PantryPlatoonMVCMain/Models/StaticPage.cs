using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

public partial class StaticPage
{
    [Key]
    public int PageId { get; set; }

    [StringLength(50)]
    public string PageName { get; set; } = null!;

    public string? HtmlContent { get; set; }
}
