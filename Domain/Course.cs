using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class Course 
	{
		public CourseDisplay CourseDetails { get; set; }
		public string Description { get; set; }
		public string ShortDescription { get; set; }

		public string Category { get; set; }

		public List<string> LearningTopics { get; set; }
		public int StudentsEnrolled		{ get; set; }


	}
}
