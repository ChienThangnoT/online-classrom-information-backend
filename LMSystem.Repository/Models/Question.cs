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
        public string? Title { get; set; }
        public string? Anwser1 { get; set; }
        public string? Anwser2 { get; set; }
        public string? Anwser3 { get; set; }
        public string? Anwser4 { get; set; }
        public string AnwserCorrect { get; set; } = string.Empty;
        public int Mark { get; set; }
        public virtual Quiz? Quiz { get; set; }
    }
}
