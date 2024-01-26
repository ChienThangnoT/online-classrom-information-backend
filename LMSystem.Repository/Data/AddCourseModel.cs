using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AddCourseModel
    {
        [Required(ErrorMessage = "Title is required!")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? VideoPreviewUrl { get; set; }

        public double? Price { get; set; }

        public double? SalesCampaign { get; set; }       

        public bool? IsPublic { get; set; }

        [Required(ErrorMessage = "Total duration is required!")]
        public int TotalDuration { get; set; }

        public bool? CourseIsActive { get; set; }

        public string? KnowdledgeDescription { get; set; }
    }
}
