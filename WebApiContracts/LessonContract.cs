using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class LessonContract
    {
        [Column("Lesson_name")]
        public string Name { get; set; }
        [Column("Lesson_description")]
        public string Description { get; set; }
        [Column("Assignment_name")]
        public string Assignment_name { get; set; }
        [Column("Assignment_description")]
        public string Assignment_description { get; set; }
        [Column("Assignment_preview")]
        public string Assignment_preview { get; set; }
        public LessonDisplay LessonDetails { get; set; }
        [Column("Assignment_file")]
        public string Assignment_file { get; set; }

        [Column("LessonStatus")]
        public string LessonStatus { get; set; }
        [Column("Lesson_Content")]
        public DateTime StartDate { get; set; }
        [Column("Lesson_Content")]
        public string Lesson_Content { get; set; }

    }
}
