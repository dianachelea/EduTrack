using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class Course 
	{	
		[Column("Name_course")]
		public string Name { get; set; }
		
		[Column("Perequisites")]
		public string Prerequisites { get; set; }
		
		[Column("Difficulty")]
		public string Difficulty { get; set; }

		[Column("ImageData")]
		public string Image { get; set; }
		
		[Column("Description")]
		public string Description { get; set; }
		
		[Column("Preview")]
		public string ShortDescription { get; set; }

		[Column("Category")]
		public string Category { get; set; }

		[Column("Learning_topics")]
		public string LearningTopics { get; set; }
		public int StudentsEnrolled { get; set; } = 0;
		
		[Column("Time")]
		public int Duration { get; set; }

		[Column("TeacherEmail")]
		public string TeacherEmail	{ get; set; }

		public void setTeacherEmail(string email) { TeacherEmail = email; }
	}
}
