using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class ReportProblem
{
    public int ReportId { get; set; }

    public string AccountId { get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ReportStatus { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ProcessingDate { get; set; }

    public virtual Account? Account { get; set; }
}
