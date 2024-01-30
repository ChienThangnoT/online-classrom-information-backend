using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Models
{
    public class AnswerHistory
    {
        public int AnswerHistoryId { get; set; }
        public int CompletedStepId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerSelected {  get; set; }
        
        public virtual Question? Question {  get; set; } 
        public virtual StepCompleted? StepCompleted { get; set; }
    }
}
