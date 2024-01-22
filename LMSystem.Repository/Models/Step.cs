using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class Step
{
    public string StepId { get; set; }

    public string SectionId { get; set; }

    public int? Duration { get; set; }

    public int? Position { get; set; }

    public string Title { get; set; }

    public string VideoUrl { get; set; }

    public string StepDescription { get; set; }

    public virtual Section Section { get; set; }
}
