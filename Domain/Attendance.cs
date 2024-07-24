using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class Attendance
    {
        //[Column("Attendance_verify")]
        public bool Attended { get; set; }
        //[Column("Email")]
        public string StudentEmail { get; set; }
        //[Column("Lesson_name")]
        public string LessonName { get; set; }
    }
}