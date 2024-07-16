using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System.Data;

namespace Infrastructure.Repositories
{
	public class CourseRepository : ICoursesRepository
	{
		private readonly IDatabaseContext _databaseContext;

		public CourseRepository(IDatabaseContext databaseContext)
		{
			this._databaseContext = databaseContext;
		}

		public async Task<bool> AddCourse(Course course)
		{
			var query = "INSERT INTO [CentricSummerPractice].[Courses] " +
				"([Name_course], [Description], [Preview], [ImageData], [Category], [Difficulty], [Time]) " +
				"VALUES (@Name, @Description, @Preview, @ImageData, @Category, @Difficulty, @Duration)";
			var parameters = new DynamicParameters();
			parameters.Add("Name", course.Name, DbType.String);
			parameters.Add("Description", course.Description, DbType.String);
			parameters.Add("Preview", course.ShortDescription, DbType.String);
			parameters.Add("Category", course.Category, DbType.String);
			parameters.Add("Difficulty", course.Difficulty, DbType.String);
			parameters.Add("Duration", course.Duration, DbType.Int32);
			parameters.Add("ImageData", course.Image, DbType.Binary);

			var connection = _databaseContext.GetDbConnection();
			var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return result != 0;
		}

		public Task<bool> DeleteCourse(string name)
		{
			throw new NotImplementedException();
		}

		public Task<List<CourseDisplay>> GetAllCourses()
		{
			throw new NotImplementedException();
		}

		public Task<Student> GetAllStudentsEnrolled(string name)
		{
			throw new NotImplementedException();
		}

		public Task<Course> GetCourse(string name)
		{
			throw new NotImplementedException();
		}

		public Task<List<CourseDisplay>> GetCoursesByFilter(CourseFilter filter)
		{
			throw new NotImplementedException();
		}

		public Task<List<Course>> GetCoursesByStudentEmail(string studentEmail)
		{
			throw new NotImplementedException();
		}

		public Task<List<CourseDisplay>> GetRelatedCourses(string name)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateCourse(string name, Course course)
		{
			throw new NotImplementedException();
		}
	}
}
