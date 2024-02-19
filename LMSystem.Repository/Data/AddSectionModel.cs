using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AddSectionModel
    {
        [Required(ErrorMessage = "CourseId is required!")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Position is required!")]
        public int Position { get; set; }
    }
}
