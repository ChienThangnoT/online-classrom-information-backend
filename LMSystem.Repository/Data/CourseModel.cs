using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Image")]

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Display(Name = "Price")]
        public double? Price { get; set; }
        [Display(Name = "Course Category")]
        public string CourseCategory { get; set; }
        [Display(Name = "Total Duration")]
        public int TotalDuration { get; set; }

        [Required(ErrorMessage = "Date of Birth is required!")]
        [Display(Name = "Update At")]
        public DateTime? UpdateAt { get; set; }
        [Display(Name = "IsPublic")]
        public bool? IsPublic { get; set; }

    }
}
