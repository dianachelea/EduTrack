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
		public string Title { get; set; }
		public List<string> Categories { get; set; }
		public List<int> Difficulties { get; set; }
		public List<String> Prerequistes { get; set; }
		public List<string> SortBy { get; set; }
	}
}
