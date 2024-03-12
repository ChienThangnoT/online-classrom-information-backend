using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class UpdateStepModel
    {
        //[Required(ErrorMessage = "StepId is required!")]
        public int StepId { get; set; }

        //[Required(ErrorMessage = "Duration is required!")]
        public int Duration { get; set; }

        //[Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "VideoUrl is required!")]
        public string VideoUrl { get; set; }
        //[Required(ErrorMessage = "Position is required!")]
        public int Position { get; set; }

        //[Required(ErrorMessage = "StepDescription is required!")]
        public string StepDescription { get; set; }

        public int QuizId { get; set; }
    }
}
