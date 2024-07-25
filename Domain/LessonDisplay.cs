using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LessonDisplay
    {
        [Column("Lesson_name")]
        public string Name { get; set; }

        [Column("Lesson_description")]
        public string Lesson_Description { get; set; }
    //    public DateTime StartDate { get; set; }
     //   [Column("LessonStatus")]
     //   public string LessonStatus { get; set; }
    }
}
