using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string QuestionTitle { get; set; } = string.Empty;
        public int CorrectAnwser { get; set; }
        public string Anwser { get; set; } = string.Empty;
        public virtual Quiz? Quiz { get; set; }
        public virtual ICollection<AnswerHistory> AnswerHistories { get; set; } = new List<AnswerHistory>();
    }
}
