using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class RatingCourse
{
    public string? RatingId { get; set; }

    public string? RegistrationId { get; set; }

    public string? CommentContent { get; set; }

    public bool? IsRatingStatus { get; set; }

    public DateTime? RatingDate { get; set; }

    public virtual RegistrationCourse? Registration { get; set; }
}
