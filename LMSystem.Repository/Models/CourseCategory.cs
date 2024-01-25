using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class CourseCategory
{
    public int CourseCategoryId { get; set; }

    public int CourseId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Course? Course { get; set; }
}
