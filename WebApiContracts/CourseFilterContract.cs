using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiContracts.Enums;

namespace WebApiContracts
{
	public class CourseFilterContract
	{
		public string Title { get; set; } = "";
		public List<string> Categories { get; set; } =	new List<string>();
		public List<string> Difficulties { get; set; } = new List<string>();
		public List<string> Prerequistes { get; set; } = new List<string>();
		public string SortBy { get; set; } = "";
	}
}
