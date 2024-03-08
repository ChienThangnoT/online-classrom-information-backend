using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class UpdateQuestionModel
    {
        [Required(ErrorMessage = "Id is required!")]
        public int QuestionId { get; set; }
        public string? QuestionTitle { get; set; }
        public int? CorrectAnwser { get; set; }
        public string? Anwser { get; set; }

    }
}
