using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class Course 
	{
		public string Name { get; set; }
		public string Prerequisites { get; set; }
		public int Difficulty { get; set; }

		//FileContentResult este specific web mvc ului asa ca putem sa salvam doar partea de bytes in loc de tot obiectul
		public Byte[] Image { get; set; }
		public string Description { get; set; }
		public string ShortDescription { get; set; }

		public string Category { get; set; }

		public List<string> LearningTopics { get; set; }
		public int StudentsEnrolled	{ get; set; }

		public string Duration { get; set; }
	}
}
