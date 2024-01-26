using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class RatingCourse
{
    public int RatingId { get; set; }

    public int RegistrationId { get; set; }

    public string? CommentContent { get; set; }
    public int RatingStar { get; set; }

    public bool? IsRatingStatus { get; set; }

    public DateTime? RatingDate { get; set; }

    public virtual RegistrationCourse? Registration { get; set; }
}
