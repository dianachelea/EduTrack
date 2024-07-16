using Domain;

namespace WebApiContracts.Mappers
{
	public static class CourseFilterMapper
	{
		public static CourseFilter MapToCourseFilter(this CourseFilterContract filterContract)
		{

			return new CourseFilter
			{
				Title = filterContract.Title,
				Categories = filterContract.Categories,
				//eroare conversie vector enum to int 
				Difficulties = filterContract.Difficulties,
				Prerequistes = filterContract.Prerequistes,
				SortBy = filterContract.SortBy
			};
		}
	}
}
