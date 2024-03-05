using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class NotificationModel
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
    }
}
