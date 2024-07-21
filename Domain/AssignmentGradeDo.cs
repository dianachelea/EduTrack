using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AssignmentGradeDo
    {
        public StudentDo Student { get; set; }
        public string Lesson_name { get; set; }
        public double Grade { get; set; }
    }
}
