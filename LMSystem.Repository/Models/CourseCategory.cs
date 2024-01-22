using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class CourseCategory
{
    public string CourseCategoryId { get; set; }

    public string CourseId { get; set; }

    public string CategoryId { get; set; }

    public virtual Category Category { get; set; }

    public virtual Course Course { get; set; }
}
