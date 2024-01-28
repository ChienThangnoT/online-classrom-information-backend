using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class UpdateSectionModel
    {
        public int SectionId { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
    }
}
