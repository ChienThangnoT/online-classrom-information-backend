using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class Course
{
    public string? CourseId { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? VideoPreviewUrl { get; set; }

    public double? Price { get; set; }

    public double? SalesCampaign { get; set; }

    public string? Title { get; set; }

    public bool? IsPublic { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? PublicAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? TotalDuration { get; set; }

    public bool? CourseIsActive { get; set; }

    public string? KnowdledgeDescription { get; set; }

    public virtual ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RegistrationCourse> RegistrationCourses { get; set; } = new List<RegistrationCourse>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();
}
