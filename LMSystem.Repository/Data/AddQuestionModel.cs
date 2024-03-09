using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AddQuestionModel
    {
        [Required(ErrorMessage = "QuizId is required!")]
        public int QuizId { get; set; }
        [Required(ErrorMessage = "QuestionTitle is required!")]
        public string QuestionTitle { get; set; }
        [Required(ErrorMessage = "CorrectAnwser is required!")]
        public int CorrectAnwser { get; set; }
        [Required(ErrorMessage = "Anwser is required!")]
        public string Anwser { get; set; }

    }
}
