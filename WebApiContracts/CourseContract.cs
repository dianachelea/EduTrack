using WebApiContracts.Enums
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApiContracts
{
	public class CourseContract
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ShortDescription { get; set; }

		public string Category { get; set; }
		public string LearningTopics { get; set; }

		public string Prerequisites { get; set; }

		public string Difficulty { get; set; }

		public IFormFile Image { get; set; }

		public int Duration	{ get; set; }

	}
}
