using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class CourseModel
    {
        public int CourseId { get; set; }

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

        public int TotalDuration { get; set; }

        public bool? CourseIsActive { get; set; }

        public string? KnowdledgeDescription { get; set; }

        public string? LinkCertificated { get; set; }
    }
}
