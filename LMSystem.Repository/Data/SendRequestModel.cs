using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public enum ReportStatus
    {
        Pending,
        Done
    }

    public enum RequestType
    {
        RequestAnAccountForParents,
        CourseReport,
        AccountReport,
        Other
    }
    public class SendRequestModel
    {

        public string AccountId { get; set; }

        public RequestType Type { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
   

    public class ResolveRequestModel
    {
        public int ReportId { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime newProCessingDay { get; set; }
    }
}
