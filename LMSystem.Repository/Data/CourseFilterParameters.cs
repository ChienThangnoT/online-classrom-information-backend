using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class CourseFilterParameters
    {
        public List<int> CategoryIds { get; set; } = new List<int>();
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; } = string.Empty;

    }
}
