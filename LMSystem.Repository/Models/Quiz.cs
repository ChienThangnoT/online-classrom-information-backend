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
        public string? Title { get; set; }
        public string? Description { get; set; } = null;
        
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Step> Steps { get; set; } = new List<Step>();
    }
}
