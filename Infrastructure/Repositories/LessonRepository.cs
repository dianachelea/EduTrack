using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IDatabaseContext _databaseContext;
        public LessonRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public async Task<IEnumerable<LessonDisplay>> GetLessons(string courseName)
        {
            var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
            var getLessonsQuery = "SELECT [Lesson_name], [Lesson_description], [Lesson_date] " +
                                  "FROM [SummerPractice].[Lessons] WHERE [Course_id] = @CourseId";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QueryFirstOrDefaultAsync<Guid>(
                getCourseIdQuery,
                new { CourseName = courseName },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == default)
                return Enumerable.Empty<LessonDisplay>(); 
            

            var lessons = await connection.QueryAsync<LessonDisplay>(
                getLessonsQuery,
                new { CourseId = courseId },
                _databaseContext.GetDbTransaction()
            );

            return lessons;
        }

        public async Task<Lesson> GetLesson(string courseName, string lessonTitle)
        {
            var getLessonQuery = @"
                SELECT * FROM [SummerPractice].[Lessons] L
                INNER JOIN [SummerPractice].[Courses] C ON L.[Course_id] = C.[Course_id]
                WHERE L.[Lesson_name] = @LessonTitle AND C.[Name_course]= @CourseName";

            var connection = _databaseContext.GetDbConnection();

            var lesson = await connection.QueryFirstOrDefaultAsync<Lesson>(
                getLessonQuery,
                new { LessonTitle = lessonTitle, CourseName=courseName },
                _databaseContext.GetDbTransaction()
            );

            return lesson;
        }

        public async Task<bool> UpdateLesson(string lessonTitle, Lesson lesson)
        {
            var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [TeacherEmail] = @TeacherEmail";
            var getTeacherEmailQuery = "SELECT TeacherEmail FROM [SummerPractice].[Courses] WHERE Course_id = (SELECT Course_id FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @LessonTitle)";
            var updateLessonQuery = "UPDATE [SummerPractice].[Lessons] " +
                                    "SET [Lesson_name] = @NewLessonName, [Lesson_description] = @LessonDescription, " +
									"[Lesson_Content] = @LessonContent, [Lesson_date] = @LessonDate " +
                                    "WHERE [Lesson_name] = @LessonTitle AND [Course_id] = @CourseId";

            var connection = _databaseContext.GetDbConnection();

            var teacherEmail = await connection.QueryFirstOrDefaultAsync<string>(
                getTeacherEmailQuery,
                new { LessonTitle = lessonTitle },
                _databaseContext.GetDbTransaction()
            );

            if (teacherEmail == default)
            {
                return false; 
            }

            var courseId = await connection.QueryFirstOrDefaultAsync<Guid>(
                getCourseIdQuery,
                new { TeacherEmail = teacherEmail },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == default)
            {
                return false; 
            }

            var parameters = new DynamicParameters();
            parameters.Add("NewLessonName", lesson.Name, DbType.String);
            parameters.Add("LessonDescription", lesson.Description, DbType.String);
           /* parameters.Add("AssignmentName", lesson.Assignment_name, DbType.String);
            parameters.Add("AssignmentDescription", lesson.Assignment_description, DbType.String);
            parameters.Add("AssignmentFile", lesson.Assignment_file, DbType.Binary);
            parameters.Add("AssignmentPreview", lesson.Assignment_preview, DbType.String);*/
            parameters.Add("LessonContent", lesson.Lesson_Content, DbType.String);
            parameters.Add("LessonDate", lesson.StartDate, DbType.DateTime);
            parameters.Add("LessonTitle", lessonTitle, DbType.String);
            parameters.Add("CourseId", courseId, DbType.Guid);

            var result = await connection.ExecuteAsync(updateLessonQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }


        public async Task<bool> AddLesson(string courseTitle, string teacherEmail, Lesson lessonData)
        {
            var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseTitle AND [TeacherEmail] = @TeacherEmail";
            var insertLessonQuery = "INSERT INTO [SummerPractice].[Lessons] " +
									"([Course_id], [Lesson_name], [Lesson_description], [Lesson_Content], [Lesson_date]) " +
									"VALUES (@Course_id, @LessonName, @LessonDescription, @LessonContent, @LessonDate)";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QueryFirstOrDefaultAsync<Guid>(
                getCourseIdQuery,
                new { CourseTitle = courseTitle, TeacherEmail = teacherEmail },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == default)
                return false; 
            

            var parameters = new DynamicParameters();
            parameters.Add("Course_id", courseId, DbType.Guid);
            parameters.Add("LessonName", lessonData.Name, DbType.String);
            parameters.Add("LessonDescription", lessonData.Description, DbType.String);
            parameters.Add("LessonContent", lessonData.Lesson_Content, DbType.String);
            parameters.Add("LessonDate", lessonData.StartDate, DbType.DateTime);

            var result = await connection.ExecuteAsync(insertLessonQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<bool> DeleteLesson(string courseName, string lessonTitle)
        {
            var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
            var deleteLessonQuery = "DELETE FROM [SummerPractice].[Lessons] WHERE [Course_id] = @CourseId AND [Lesson_name] = @LessonTitle";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QueryFirstOrDefaultAsync<Guid>(
                getCourseIdQuery,
                new { CourseName = courseName, },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == default)
            {
                return false; 
            }

            var parameters = new DynamicParameters();
            parameters.Add("CourseId", courseId, DbType.Guid);
            parameters.Add("LessonTitle", lessonTitle, DbType.String);

            var result = await connection.ExecuteAsync(deleteLessonQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<bool> ChangeStatus(string courseName, string lessonTitle, string status, string teacherEmail)
        {
            var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName AND [TeacherEmail]= @TeacherEmail";
            var updateLessonStatusQuery = "UPDATE [SummerPractice].[Lessons] " +
                                          "SET [LessonStatus] = @Status " +
                                          "WHERE [Lesson_name] = @LessonTitle AND [Course_id] = @CourseId";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QueryFirstOrDefaultAsync<Guid>(
                getCourseIdQuery,
                new { CourseName = courseName, TeacherEmail = teacherEmail },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == default)
            {
                return false; 
            }

            var parameters = new DynamicParameters();
            parameters.Add("Status", status, DbType.String);
            parameters.Add("LessonTitle", lessonTitle, DbType.String);
            parameters.Add("CourseId", courseId, DbType.Guid);

            var result = await connection.ExecuteAsync(updateLessonStatusQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

    }
}
