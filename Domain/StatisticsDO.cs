using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class StatisticsDO
    {
        public int? CoursesCompleted {  get; set; }     // for teachers it means lesson attendance
        public Dictionary<string, int>? AllCoursesCompleted {  get; set; }     // Dict<CourseName, %lessonAttendance>
        public int? AssignmentsCompleted { get; set; }
        public Dictionary<string, int>? AllAssignmentsCompleted { get; set; }   // Dict<CourseName, %done_assignments>
        public int? CoursesGrade { get; set; }
        public Dictionary<string, int>? AllCoursesGrades { get; set; }      // Dict<CourseName, courseGrade>
    }
}
