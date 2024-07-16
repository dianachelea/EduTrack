using WebApiContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
	public class CourseContract
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ShortDescription { get; set; }

		public string Category { get; set; }
		public List<string> LearningTopics { get; set; }

		public string Prerequisites { get; set; }

		public CourseDifficulty Difficulty { get; set; }

		//take the byte array from the result
		public Byte[] Image { get; set; }

		public string Duration	{ get; set; }

	}
}
