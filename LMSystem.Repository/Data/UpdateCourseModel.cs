using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class UpdateCourseModel
    {
        //[Required(ErrorMessage = "CourseId is required!")]
        public int CourseId { get; set; }
        //[Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        //[Required(ErrorMessage = "ImageUrl is required!")]
        public string ImageUrl { get; set; }
        public string VideoPreviewUrl { get; set; }
        //[Required(ErrorMessage = "Price is required!")]
        public double Price { get; set; }
        //[Required(ErrorMessage = "SalesCampaign is required!")]
        public double SalesCampaign { get; set; }
        public bool IsPublic { get; set; }
        //[Required(ErrorMessage = "Total duration is required!")]
        public int TotalDuration { get; set; }

        public bool CourseIsActive { get; set; }
        //[Required(ErrorMessage = "KnowdledgeDescription is required!")]
        public string KnowdledgeDescription { get; set; }

        //[Required(ErrorMessage = "LinkCertificated is required!")]
        public string LinkCertificated {  get; set; }

        //[Required(ErrorMessage = "CategoryList is required!")]
        public List<int> CategoryList { get; set; }
    }
}
