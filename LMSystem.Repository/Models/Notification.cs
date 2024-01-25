using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class Notification
{
    public string? NotificationId { get; set; }

    public string? AccountId { get; set; }

    public string? Message { get; set; }

    public DateTime? SendDate { get; set; }

    public virtual Account? Account { get; set; }
}
