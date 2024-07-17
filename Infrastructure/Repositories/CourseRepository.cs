using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Repositories
{
	public class CourseRepository : ICoursesRepository
	{
		private readonly IDatabaseContext _databaseContext;

		public CourseRepository(IDatabaseContext databaseContext)
		{
			this._databaseContext = databaseContext;
		}

		public async Task<bool> AddCourse(string email, Course course)
		{
			var query = "INSERT INTO [CentricSummerPractice].[Courses] " +
				"([Name_course], [Description], [Preview], [ImageData], [Category], [Difficulty], [Time], [Learning_topics], [Perequisities], [TeacherEmail]) " +
				"VALUES (@Name, @Description, @Preview, @ImageData, @Category, @Difficulty, @Duration, @LearningTopics, @Prerequisites, @TeacherEmail)";
			var parameters = new DynamicParameters();
			parameters.Add("Name", course.Name, DbType.String);
			parameters.Add("Description", course.Description, DbType.String);
			parameters.Add("Preview", course.ShortDescription, DbType.String);
			parameters.Add("Category", course.Category, DbType.String);
			parameters.Add("Difficulty", course.Difficulty, DbType.String);
			parameters.Add("Duration", course.Duration, DbType.Int32);
			parameters.Add("ImageData", course.Image, DbType.Binary);
			parameters.Add("LearningTopics", course.LearningTopics, DbType.String);
			parameters.Add("Prerequisites", course.Prerequisites, DbType.String);
			parameters.Add("TeacherEmail", email, DbType.String);

			var connection = _databaseContext.GetDbConnection();
			var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return result != 0;
		}

		public async Task<bool> DeleteCourse(string email, string name)
		{
			var query = "DELETE FROM [CentricSummerPractice].[Courses] WHERE [Name_course] = @Name AND [TeacherEmail]= @TeacherEmail";
			var parameters = new DynamicParameters();
			parameters.Add("Name", name, DbType.String);
			parameters.Add("TeacherEmail", email, DbType.String);

			var connection = _databaseContext.GetDbConnection();
			var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return result != 0;

		}

		public Task<IEnumerable<CourseDisplay>> GetAllCourses()
		{
			
			var query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [CentricSummerPractice].[Courses]";

			var connection = _databaseContext.GetDbConnection();
			var courses = connection.QueryAsync<CourseDisplay>(query);
			return courses;

		}
		public Task<IEnumerable<string>> GetTeacherCourses(string email)
		{

			var query = "SELECT [Name_course] FROM [CentricSummerPractice].[Courses] WHERE [TeacherEmail]=@TeacherEmail";

			var connection = _databaseContext.GetDbConnection();
			
			var coursesNames = connection.QueryAsync<string>(query, new { TeacherEmail = email });
			return coursesNames;

		}

		public async Task<IEnumerable<Student>> GetAllStudentsEnrolled(string name)
		{
			//get course id based on its name in c
			var getId = "SELECT [Course_id] FROM [CentricSummerPractice].[Courses] WHERE [Name_course] = @Name";

			var connection = _databaseContext.GetDbConnection();
			var parameters = new DynamicParameters();
			parameters.Add("Name", name, DbType.String);
			var result = await connection.ExecuteAsync(getId, parameters, _databaseContext.GetDbTransaction());

			
			//get students emails based on course id in s-c
			var getEmails = "SELECT [User].[First_name], [User].[Last_name], [User].[Email] FROM [CentricSummerPractice].[Students-Courses] LEFT JOIN [CentricSummerPractice].[User]" +
					"ON  [CentricSummerPractice].[Students-Courses].[Email] =  [CentricSummerPractice].[User].[Email] WHERE [Course_id]= @id";

			//parameters = new DynamicParameters();
			//parameters.Add("id", result);
			var studentsResult = await connection.QueryAsync<Student>(getEmails, new { id = result }, _databaseContext.GetDbTransaction());

			return studentsResult;
			
		}

		public Task<IEnumerable<Course>> GetCourse(string name)
		{
			var query = "SELECT * FROM [CentricSummerPractice].[Courses] WHERE [Name_course] = @Name";

			var connection = _databaseContext.GetDbConnection();
			var course = connection.QueryAsync<Course>(query, new { Name = name });
			return course;
		}

		public Task<IEnumerable<CourseDisplay>> GetCoursesByFilter(CourseFilter filter)
		{
			//throw new NotImplementedException();
			var query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [CentricSummerPractice].[Courses]" +
				"WHERE [Name_course] = @Name OR [Category] IN @Categories OR [Difficulty] IN @Difficulties OR [Perequisties] IN @Prerequisites SORT BY @Sort";

			var connection = _databaseContext.GetDbConnection();
			var parameters = new DynamicParameters();
			parameters.Add("Name", filter.Title, DbType.String);
			parameters.Add("Categories", filter.Categories);
			parameters.Add("Difficulties", filter.Difficulties);
			parameters.Add("Prerequisites", filter.Prerequistes);
			parameters.Add("Sort", filter.SortBy); //asc or desc

			var filterResults = connection.QueryAsync<CourseDisplay>(query, parameters, _databaseContext.GetDbTransaction());
			return filterResults;

		}

		public Task<IEnumerable<Course>> GetCoursesByStudentEmail(string studentEmail)
		{
			var query = "SELECT * FROM  [CentricSummerPractice].[Courses] LEFT JOIN [CentricSummerPractice].[Students-Courses] " +
				"ON  [CentricSummerPractice].[Courses].[Course_id] = [CentricSummerPractice].[Students-Courses].[Course_id] " +
				"WHERE [CentricSummerPractice].[Students-Courses].[Email] = @Email";

			var connection = _databaseContext.GetDbConnection();
			var courses = connection.QueryAsync<Course>(query);
			return courses;
		}

		public Task<IEnumerable<CourseDisplay>> GetRelatedCourses(string name)
		{

			var query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [CentricSummerPractice].[Courses]" +
				" WHERE [Category] = (SELECT [Category] FROM [CentricSummerPractice].[Courses] WHERE [Name_Course] = @Name";

			var connection = _databaseContext.GetDbConnection();
			var course = connection.QueryAsync<CourseDisplay>(query, new { Name = name });
			return course;
		}

		public async Task<bool> UpdateCourse(string email, string name, Course course)
		{
			var query = "UPDATE [CentricSummerPractice].[Courses] " +
				"SET [Name_course] = @NewName, [Description] = @Description, [Preview] = @Preview, [ImageData] = @ImageData, " +
				"[Category] = @Category, [Difficulty] = @Difficulty," +
				"[Time] = @Duration, [Learning_topics] = @LearningTopics,[Perequisities] = @Prerequisites" +
				"WHERE [Name_course] = @Name AND [TeacherEmail]= @TeacherEmail";

			var parameters = new DynamicParameters();
			parameters.Add("NewName", name, DbType.String);
			parameters.Add("Name", course.Name, DbType.String);
			parameters.Add("Description", course.Description, DbType.String);
			parameters.Add("Preview", course.ShortDescription, DbType.String);
			parameters.Add("Category", course.Category, DbType.String);
			parameters.Add("Difficulty", course.Difficulty, DbType.String);
			parameters.Add("Duration", course.Duration, DbType.Int32);
			parameters.Add("ImageData", course.Image, DbType.Binary);
			parameters.Add("LearningTopics", course.LearningTopics, DbType.String);
			parameters.Add("Prerequisites", course.Prerequisites, DbType.String);
			parameters.Add("TeacherEmail", email, DbType.String);

			var connection = _databaseContext.GetDbConnection();
			var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return result != 0;

		}

		public async Task<IEnumerable<Student>> GetStudentsEnrolledInCourse(string name, string teacherEmail)
		{
			var query = "SELECT Email FROM [CentricSummerPractice].[Students-Courses] " +
				"WHERE Course_id = (SELECT Course_id FROM [CentricSummerPractice].[Students-Courses] WHERE Name_course = @Name" +
				"AND TeacherEmail = @TeacherEmail";

			var parameters = new DynamicParameters();
			parameters.Add("Name", name, DbType.String);
			parameters.Add("TeacherEmail", teacherEmail, DbType.String);
			
			var connection = _databaseContext.GetDbConnection();
			var emails = connection.Query<string>(query, parameters, _databaseContext.GetDbTransaction());

			query = "SELECT First_name, Last_Name, Email FORM [CentricSummerPractice].[User] WHERE Email IN @Emails";
			var students = connection.Query<Student>(query, new { Emails = emails }, _databaseContext.GetDbTransaction());

			return students;

		}
	}
}
