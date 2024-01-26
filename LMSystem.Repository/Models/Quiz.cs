using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int StepId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; } = null;
        public virtual Step? Step { get; set; }

        public virtual ICollection<Question> Question { get; set; } = new List<Question>();
    }
}
