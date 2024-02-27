using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AddRatingModel
    {
        [Range(1, 5)]
        public int RatingStar { get; set; }

        public string? CommentContent { get; set; }
    }
}
