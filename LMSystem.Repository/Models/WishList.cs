using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class WishList
{
    public string? WishListId { get; set; }

    public string? CourseId { get; set; }

    public string? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Course? Course { get; set; }
}
