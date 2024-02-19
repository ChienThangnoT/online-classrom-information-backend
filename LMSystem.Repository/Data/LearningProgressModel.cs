using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class LearningProgressModel
    {
        public string CurrentSection { get; set; }
        public string CurrentStep { get; set; }
        public int? CurrentStepPosition { get; set; }
        public double ProgressPercentage { get; set; } 
        public bool? IsCompleted { get; set; }
        public DateTime EnrollDay { get; set; }
        public DateTime StepCompleteDay { get; set; }
    }

}
