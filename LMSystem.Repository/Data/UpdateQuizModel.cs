using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class UpdateQuizModel
    {
        public int QuizId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<UpdateQuestion> Questions { get; set; } = new List<UpdateQuestion>();

    }

    public class UpdateQuestion
    {
        public string? QuestionTitle { get; set; } 
        public int? CorrectAnwser { get; set; }
        public string? Anwser { get; set; }
    }
}
