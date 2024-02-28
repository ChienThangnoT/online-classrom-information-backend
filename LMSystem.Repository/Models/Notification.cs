using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string AccountId { get; set; }

    public DateTime? SendDate { get; set; }

    public string? Type { get; set; }

    public bool? IsRead { get; set; } = false;

    public string? Title { get; set; }

    public string? Action { get; set; }

    public string? Message { get; set; }

    public int? ModelId { get; set; }

    public virtual Account? Account { get; set; }
}
