using Domain;

namespace WebApiContracts.Mappers
{
	public static class CourseMapper
	{

		public static Course MapToCourse(this CourseContract courseContract)
		{
			return new Course
			{
				Name = courseContract.Name,
				Prerequisites = courseContract.Prerequisites,
				Description = courseContract.Description,
				ShortDescription = courseContract.ShortDescription,
				LearningTopics = courseContract.LearningTopics,
				Category = courseContract.Category,
				Duration = courseContract.Duration,
				Difficulty = courseContract.Difficulty,
				Image = courseContract.Image.FileName,
				

			};
		}

	}
}
