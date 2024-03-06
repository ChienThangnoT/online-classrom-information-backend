using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class StudentPerCourseModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int TotalStudents { get; set; }
    }
}
