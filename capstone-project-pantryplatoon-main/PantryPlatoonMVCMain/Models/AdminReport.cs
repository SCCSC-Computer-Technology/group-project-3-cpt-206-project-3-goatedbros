using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PantryPlatoonMVCMain.Models;

public partial class AdminReport
{
    [Key]
    public int ReportId { get; set; }

    public DateOnly Month { get; set; }

    public int TotalVisits { get; set; }

    public int TotalStudentsServed { get; set; }

    public int RepeatVisitors { get; set; }

    public int TotalItems { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalWeight { get; set; }
}
