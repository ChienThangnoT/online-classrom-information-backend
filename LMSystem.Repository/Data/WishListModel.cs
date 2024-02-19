using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class WishListModel
    {
        public int WishListId { get; set; }

        public int CourseId { get; set; }

        public string? AccountId { get; set; }
    }
}
