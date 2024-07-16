

namespace Domain
{
	public class CourseFilter
	{
		public string Title { get; set; }
		public List<string> Categories { get; set; }
		public List<int> Difficulties { get; set; }
		public List<String> Prerequistes { get; set; }
		public List<string> SortBy { get; set; }

	}
}
