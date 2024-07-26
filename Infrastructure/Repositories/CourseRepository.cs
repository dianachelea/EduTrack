using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure.Repositories
{
	public class CourseRepository : ICoursesRepository
	{
		private readonly IDatabaseContext _databaseContext;

		public CourseRepository(IDatabaseContext databaseContext)
		{
			this._databaseContext = databaseContext;
		}


		public async Task<bool> AddCourse(Course course) //works
		{
			var query = "INSERT INTO [SummerPractice].[Courses] " +
				"([Name_course], [Description], [Preview], [ImageData], [Category], [Difficulty], [Time], [Learning_topics], [Perequisites], [TeacherEmail]) " +
				"VALUES (@Name, @Description, @Preview, @ImageData, @Category, @Difficulty, @Duration, @LearningTopics, @Prerequisites, @TeacherEmail)";
			var parameters = new DynamicParameters();
			parameters.Add("Name", course.Name, DbType.String);
			parameters.Add("Description", course.Description, DbType.String);
			parameters.Add("Preview", course.ShortDescription, DbType.String);
			parameters.Add("Category", course.Category, DbType.String);
			parameters.Add("Difficulty", course.Difficulty, DbType.String);
			parameters.Add("Duration", course.Duration, DbType.String);
			parameters.Add("ImageData", course.Image, DbType.String);
			parameters.Add("LearningTopics", course.LearningTopics, DbType.String);
			parameters.Add("Prerequisites", course.Prerequisites, DbType.String);
			parameters.Add("TeacherEmail", course.TeacherEmail);

			var connection = _databaseContext.GetDbConnection();
			var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return result != 0;
		}

		public async Task<bool> DeleteCourse(string email, string name) //works
		{


			var query = "DELETE FROM [SummerPractice].[Courses] WHERE [Name_course] = @Name AND [TeacherEmail]= @TeacherEmail";

			var connection = _databaseContext.GetDbConnection();
			var parameters = new DynamicParameters();
			parameters.Add("Name", name, DbType.String);
			parameters.Add("TeacherEmail", email, DbType.String);
			var finalResult = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());

			return finalResult != 0;

		}

		public async Task<bool> UpdateCourse(string email, string name, Course course) //works
		{
			var query = "UPDATE [SummerPractice].[Courses] " +
				"SET [Name_course] = @NewName, [Description] = @Description, [Preview] = @Preview, [ImageData] = @ImageData, " +
				"[Category] = @Category, [Difficulty] = @Difficulty," +
				"[Time] = @Duration, [Learning_topics] = @LearningTopics,[Perequisites] = @Prerequisites " +
				"WHERE [Name_course] = @Name AND [TeacherEmail]= @TeacherEmail";

			var parameters = new DynamicParameters();
			parameters.Add("Name", name, DbType.String);
			parameters.Add("NewName", course.Name, DbType.String);
			parameters.Add("Description", course.Description, DbType.String);
			parameters.Add("Preview", course.ShortDescription, DbType.String);
			parameters.Add("Category", course.Category, DbType.String);
			parameters.Add("Difficulty", course.Difficulty, DbType.String);
			parameters.Add("Duration", course.Duration, DbType.Int32);
			parameters.Add("ImageData", course.Image, DbType.String);
			parameters.Add("LearningTopics", course.LearningTopics, DbType.String);
			parameters.Add("Prerequisites", course.Prerequisites, DbType.String);
			parameters.Add("TeacherEmail", email, DbType.String);

			var connection = _databaseContext.GetDbConnection();
			var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return result != 0;

		}

		public IEnumerable<CourseDisplay> GetAllCourses() //works
		{

			var query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [SummerPractice].[Courses]";

			var connection = _databaseContext.GetDbConnection();
			var courses = connection.Query<CourseDisplay>(query);
			return courses;

		}


/*		public IEnumerable<CourseDisplay> GetSortedCourses(string order) //works
		{
			var query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [SummerPractice].[Courses]";
			var sortDirection = order.ToLower() == "desc" ? "DESC" : "ASC";
			query += " ORDER BY [Name_course] " + sortDirection;

			var connection = _databaseContext.GetDbConnection();
			var courses = connection.Query<CourseDisplay>(query);
			return courses;
		}*/

		public IEnumerable<CourseDisplay> GetCoursesByFilter(CourseFilter filter) //works
		{
			var query = "";
			var connection = _databaseContext.GetDbConnection();
			var parameters = new DynamicParameters();

			query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [SummerPractice].[Courses]" +
			"WHERE [Name_course] LIKE @Name ";
			//OR [Difficulty] IN @Difficulties OR [Category] IN @Categories
			if (filter.Prerequistes.Any())
				query += ConstructFilter(filter.Prerequistes, "Perequisites", ref parameters);
			if (filter.Difficulties.Any())
				query += ConstructFilter(filter.Difficulties, "Difficulty", ref parameters);
			if (filter.Categories.Any())
				query += ConstructFilter(filter.Categories, "Category", ref parameters);

			if (filter.SortBy != "")
			{
				var sortDirection = filter.SortBy.ToLower() == "desc" ? "DESC" : "ASC";
				query += " ORDER BY [Name_course] " + sortDirection;
			}

			parameters.Add("Name", '%' + filter.Title + '%', DbType.String);
			

			var filterResults = connection.Query<CourseDisplay>(query, parameters);
			return filterResults;
		}

		private string ConstructFilter(List<string> filter, string columnName, ref DynamicParameters parameters)
		{
			string query = "";
			query += $" AND [{columnName}] IN ( ";
			for (int index = 0; index < filter.Count() - 1; index++)
			{
				query += $"@{columnName}{index}, ";
				parameters.Add($"{columnName}{index}", filter[index], DbType.String);
			}
			query += $"@{columnName}{filter.Count() - 1}";
			parameters.Add($"{columnName}{filter.Count() - 1}", filter.Last(), DbType.String);
			query += " ) ";

			return query;
		}


		public Task<IEnumerable<string>> GetTeacherCourses(string email) //works in sql
		{

			var query = "SELECT [Name_course] FROM [SummerPractice].[Courses] WHERE [TeacherEmail]=@TeacherEmail";

			var connection = _databaseContext.GetDbConnection();

			var coursesNames = connection.QueryAsync<string>(query, new { TeacherEmail = email });
			return coursesNames;

		}

		public IEnumerable<Course> GetCourse(string name) //works
		{
			var query = "SELECT * FROM [SummerPractice].[Courses] WHERE [Name_course] = @Name";

			var connection = _databaseContext.GetDbConnection();
			var course = connection.Query<Course>(query, new { Name = name });
			return course;
		}

		public IEnumerable<CourseInfoPage> GetCourseForPage(string name) //works
		{
			var query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview], [Description], [Time], [Learning_topics], [Category] FROM [SummerPractice].[Courses] WHERE [Name_course] = @Name";

			var connection = _databaseContext.GetDbConnection();
			var course = connection.Query<CourseInfoPage>(query, new { Name = name });
			return course;
		}


		public IEnumerable<Course> GetCoursesByStudentEmail(string studentEmail) //works
		{
			var query = "SELECT * FROM  [SummerPractice].[Courses] LEFT JOIN [SummerPractice].[Students-Courses] " +
				"ON  [SummerPractice].[Courses].[Course_id] = [SummerPractice].[Students-Courses].[Course_id] " +
				"WHERE [SummerPractice].[Students-Courses].[Email] = @Email";

			var connection = _databaseContext.GetDbConnection();
			var courses = connection.Query<Course>(query, new { Email = studentEmail });
			return courses;
		}

		public IEnumerable<CourseDisplay> GetRelatedCourses(string name) //works
		{

			var query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [SummerPractice].[Courses]" +
				" WHERE [Category] = (SELECT [Category] FROM [SummerPractice].[Courses] WHERE [Name_Course] = @Name)" +
				" AND NOT [Name_course] = @Name ";

			var connection = _databaseContext.GetDbConnection();
			var course = connection.Query<CourseDisplay>(query, new { Name = name });
			return course;
		}



		public IEnumerable<Student> GetStudentsEnrolledInCourse(string name, string teacherEmail) //works
		{
			var query = "SELECT [Email] FROM [SummerPractice].[Students-Courses] " +
				"WHERE [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @Name " +
				"AND [TeacherEmail]=@TeacherEmail)";

			var parameters = new DynamicParameters();
			parameters.Add("Name", name, DbType.String);
			parameters.Add("TeacherEmail", teacherEmail, DbType.String);

			var connection = _databaseContext.GetDbConnection();
			var emails = connection.Query<string>(query, parameters, _databaseContext.GetDbTransaction());

			query = "SELECT [First_name], [Last_Name],[Email] FROM [SummerPractice].[User] WHERE [Email] IN @Emails";
			var students = connection.Query<Student>(query, new { Emails = emails }, _databaseContext.GetDbTransaction());


			return students;

		}

		public IEnumerable<Lesson> GetCourseLessons(string name, string teacherEmail) //works in sql
		{
			var query = "SELECT [Lesson_name], [Lesson_description], [Lesson_content] FROM [SummerPractice].[Lessons] " +
				"WHERE [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course]= @Name AND [TeacherEmail] = @Email";

			var connection = _databaseContext.GetDbConnection();

			var parameters = new DynamicParameters();
			parameters.Add("Name", name, DbType.String);
			parameters.Add("Email", teacherEmail, DbType.String);

			var result = connection.Query<Lesson>(query, parameters, _databaseContext.GetDbTransaction());

			return result;
		}

		public IEnumerable<CourseDisplay> GetCoursesWithMaxEnrollment(int numberOfCourses) //works
		{

			var query = $"SELECT TOP {numberOfCourses} [Course_id] FROM [SummerPractice].[Students-Courses]" +
				" GROUP BY [Course_id]" +
				" ORDER BY COUNT([Email]) DESC";

			var connection = _databaseContext.GetDbConnection();

			var ids = connection.Query<Guid>(query).ToList();

			query = "SELECT [Name_course], [Perequisites], [Difficulty], [ImageData], [Preview] FROM [SummerPractice].[Courses]" +
				" WHERE [Course_id] IN @Ids";
			var courses = connection.Query<CourseDisplay>(query, new { Ids = ids });

			return courses;
		}

		public Task<int> EnrollStudent(string courseName, string studentEmail) //works
		{
			var query = "SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @Name";
			var connection = _databaseContext.GetDbConnection();
			var id = connection.Query<Guid>(query, new { Name = courseName });

			query = "INSERT INTO [SummerPractice].[Students-Courses] ([Email], [Course_id]) VALUES (@Email, @CourseId)";

			var parameters = new DynamicParameters();
			parameters.Add("CourseId", id);
			parameters.Add("Email", studentEmail);

			var result = connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return result;
		}

		public IEnumerable<Attendance> GetStudentAttendance(string courseName, string studentEmail) //works
		{
			var query = "SELECT [SummerPractice].[Attendance].[Email] , [SummerPractice].[Attendance].[Attendance_verify], " +
				"[SummerPractice].[Lessons].[Lesson_name] FROM [SummerPractice].[Attendance] INNER JOIN " +
				"[SummerPractice].[Lessons] ON [SummerPractice].[Attendance].[Lesson_id] = [SummerPractice].[Lessons].[Lesson_id] " +
				"WHERE [SummerPractice].[Attendance].[Email] = @Email AND [SummerPractice].[Lessons].[Course_id] = " +
				"(SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @Name)";

			var connection = _databaseContext.GetDbConnection();
			var parameters = new DynamicParameters();
			parameters.Add("Name", courseName);
			parameters.Add("Email", studentEmail);

			var result = connection.Query<Attendance>(query, parameters);
			return result;
		}



	}
}
