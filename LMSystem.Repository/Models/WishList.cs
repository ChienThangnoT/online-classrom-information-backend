using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class WishList
{
    public int WishListId { get; set; }

    public int CourseId { get; set; }

    public string AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Course? Course { get; set; }
}
