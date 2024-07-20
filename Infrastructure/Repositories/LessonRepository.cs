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
            var getCourseIdQuery = "SELECT Id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
            var getLessonsQuery = "SELECT [Lesson_name] , [Lesson_Content] ,[LessonStatus] " +
                                  "FROM [SummerPractice].[Lessons] WHERE [Course_id] = @CourseId";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QuerySingleOrDefaultAsync<int>(
                getCourseIdQuery,
                new { CourseName = courseName },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == 0)
            {
                return Enumerable.Empty<LessonDisplay>(); 
            }

            var lessons = await connection.QueryAsync<LessonDisplay>(
                getLessonsQuery,
                new { CourseId = courseId },
                _databaseContext.GetDbTransaction()
            );

            return lessons;
        }

        public async Task<Lesson> GetLesson(string lessonTitle, string teacherEmail)
        {
            var getLessonQuery = @"
        SELECT * FROM [SummerPractice].[Lessons] L
        INNER JOIN [SummerPractice].[Courses] C ON L.[Course_id] = C.[Id]
        WHERE L.[Lesson_name] = @LessonTitle AND C.[TeacherEmail] = @TeacherEmail";

            var connection = _databaseContext.GetDbConnection();

            var lesson = await connection.QuerySingleOrDefaultAsync<Lesson>(
                getLessonQuery,
                new { LessonTitle = lessonTitle, TeacherEmail = teacherEmail },
                _databaseContext.GetDbTransaction()
            );

            return lesson;
        }

        public async Task<bool> UpdateLesson(string lessonTitle, Lesson lesson)
        {
            var getCourseIdQuery = "SELECT Id FROM [SummerPractice].[Courses] WHERE [TeacherEmail] = @TeacherEmail";
            var getTeacherEmailQuery = "SELECT TeacherEmail FROM [SummerPractice].[Courses] WHERE Id = (SELECT Course_id FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @LessonTitle)";
            var updateLessonQuery = "UPDATE [SummerPractice].[Lessons] " +
                                    "SET [Lesson_name] = @NewLessonName, [Lesson_description] = @LessonDescription, " +
                                    "[Assignment_name] = @AssignmentName, [Assignment_description] = @AssignmentDescription, " +
                                    "[Assignment_file] = @AssignmentFile, [Assignment_preview] = @AssignmentPreview, " +
                                    "[Lesson_Content] = @LessonContent " +
                                    "WHERE [Lesson_name] = @LessonTitle AND [Course_id] = @CourseId";

            var connection = _databaseContext.GetDbConnection();

            var teacherEmail = await connection.QuerySingleOrDefaultAsync<string>(
                getTeacherEmailQuery,
                new { LessonTitle = lessonTitle },
                _databaseContext.GetDbTransaction()
            );

            if (teacherEmail == null)
            {
                return false; 
            }

            var courseId = await connection.QuerySingleOrDefaultAsync<int>(
                getCourseIdQuery,
                new { TeacherEmail = teacherEmail },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == 0)
            {
                return false; 
            }

            var parameters = new DynamicParameters();
            parameters.Add("NewLessonName", lesson.Name, DbType.String);
            parameters.Add("LessonDescription", lesson.Description, DbType.String);
            parameters.Add("AssignmentName", lesson.Assignment_name, DbType.String);
            parameters.Add("AssignmentDescription", lesson.Assignment_description, DbType.String);
            parameters.Add("AssignmentFile", lesson.Assignment_file, DbType.Binary);
            parameters.Add("AssignmentPreview", lesson.Assignment_preview, DbType.String);
            parameters.Add("LessonContent", lesson.Lesson_Content, DbType.String);
            parameters.Add("LessonTitle", lessonTitle, DbType.String);
            parameters.Add("CourseId", courseId, DbType.Int32);

            var result = await connection.ExecuteAsync(updateLessonQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }


        public async Task<bool> AddLesson(string courseTitle, string teacherEmail, Lesson lessonData)
        {
            var getCourseIdQuery = "SELECT Id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseTitle AND [TeacherEmail] = @TeacherEmail";
            var insertLessonQuery = "INSERT INTO [SummerPractice].[Lessons] " +
                                    "([Course_id], [Lesson_name], [Lesson_description], [Assignment_name], [Assignment_description], [Assignment_file], [Assignment_preview], [Lesson_Content]) " +
                                    "VALUES (@CourseId, @LessonName, @LessonDescription, @AssignmentName, @AssignmentDescription, @AssignmentFile, @AssignmentPreview, @LessonContent)";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QuerySingleOrDefaultAsync<int>(
                getCourseIdQuery,
                new { CourseTitle = courseTitle, TeacherEmail = teacherEmail },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == 0)
            {
                return false; 
            }

            var parameters = new DynamicParameters();
            parameters.Add("CourseId", courseId, DbType.Int32);
            parameters.Add("LessonName", lessonData.Name, DbType.String);
            parameters.Add("LessonDescription", lessonData.Description, DbType.String);
            parameters.Add("AssignmentName", lessonData.Assignment_name, DbType.String);
            parameters.Add("AssignmentDescription", lessonData.Assignment_description, DbType.String);
            parameters.Add("AssignmentFile", lessonData.Assignment_file, DbType.Binary);
            parameters.Add("AssignmentPreview", lessonData.Assignment_preview, DbType.String);
            parameters.Add("LessonContent", lessonData.Lesson_Content, DbType.String);

            var result = await connection.ExecuteAsync(insertLessonQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<bool> DeleteLesson(string courseName, string lessonTitle)
        {
            var getCourseIdQuery = "SELECT Id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
            var deleteLessonQuery = "DELETE FROM [SummerPractice].[Lessons] WHERE [Course_id] = @CourseId AND [Lesson_name] = @LessonTitle";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QuerySingleOrDefaultAsync<int>(
                getCourseIdQuery,
                new { CourseName = courseName },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == 0)
            {
                return false; 
            }

            var parameters = new DynamicParameters();
            parameters.Add("CourseId", courseId, DbType.Int32);
            parameters.Add("LessonTitle", lessonTitle, DbType.String);

            var result = await connection.ExecuteAsync(deleteLessonQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<bool> ChangeStatus(string courseName, string lessonTitle, string status)
        {
            var getCourseIdQuery = "SELECT Id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
            var updateLessonStatusQuery = "UPDATE [SummerPractice].[Lessons] " +
                                          "SET [LessonStatus] = @Status " +
                                          "WHERE [Lesson_name] = @LessonTitle AND [Course_id] = @CourseId";

            var connection = _databaseContext.GetDbConnection();

            var courseId = await connection.QuerySingleOrDefaultAsync<int>(
                getCourseIdQuery,
                new { CourseName = courseName },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == 0)
            {
                return false; 
            }

            var parameters = new DynamicParameters();
            parameters.Add("Status", status, DbType.String);
            parameters.Add("LessonTitle", lessonTitle, DbType.String);
            parameters.Add("CourseId", courseId, DbType.Int32);

            var result = await connection.ExecuteAsync(updateLessonStatusQuery, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

    }
}
