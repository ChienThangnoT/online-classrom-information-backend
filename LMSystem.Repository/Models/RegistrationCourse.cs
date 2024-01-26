using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class RegistrationCourse
{
    public int RegistrationId { get; set; }

    public int CourseId { get; set; }

    public string AccountId { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public bool? IsCompleted { get; set; }

    public double? LearningProgress { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<RatingCourse> RatingCourses { get; set; } = new List<RatingCourse>();

    public virtual ICollection<StepCompleted> StepCompleteds { get; set; } = new List<StepCompleted>();
}
