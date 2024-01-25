using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public int CourseId { get; set; }

    public string? Title { get; set; }

    public int? Position { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();
}
