using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class Account : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Biography { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? ProfileImg { get; set; }

    public string? Sex { get; set; }
    public string? ParentEmail { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? DeviceToken { get; set; }
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RegistrationCourse> RegistrationCourses { get; set; } = new List<RegistrationCourse>();

    public virtual ICollection<ReportProblem> ReportProblems { get; set; } = new List<ReportProblem>();

    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();

    public virtual ICollection<LinkCertificateAccount> LinkCertificateAccounts { get; set; } = new List<LinkCertificateAccount>();

}
