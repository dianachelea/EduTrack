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
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly IDatabaseContext _databaseContext;
        public AttendanceRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<List<Attendance>> GetStudentAttendance(string courseName, string studentEmail)
        {
			var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
			var getAttendanceQuery = @"
                SELECT L.[Lesson_name], A.[Attendance_verify], U.[Email] AS Email
                FROM [SummerPractice].[Attendance] A
                INNER JOIN [SummerPractice].[User] U ON A.[Email] = U.[Email] 
                INNER JOIN [SummerPractice].[Lessons] L ON A.[Lesson_id] = L.[Lesson_id]
                WHERE U.[Email] =  @StudentEmail AND L.[Course_id] = @CourseId";

            var connection = _databaseContext.GetDbConnection();

			var courseId = await connection.QuerySingleOrDefaultAsync<Guid>(
				getCourseIdQuery,
				new { CourseName = courseName },
				_databaseContext.GetDbTransaction()
			);
			if (courseId == default)
				return new List<Attendance>();

			var attendanceRecords = await connection.QueryAsync<Attendance>(
                getAttendanceQuery,
                new { StudentEmail = studentEmail, CourseId = courseId },
                _databaseContext.GetDbTransaction()
            );

            return attendanceRecords.ToList();
        }
        public async Task<bool> MakeAttendance(string courseName, string lessonTitle, List<Student> students)
        {
            var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
            var getLessonIdQuery = "SELECT Lesson_id FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @LessonTitle AND [Course_id] = @CourseId";
            var insertAttendanceQuery = @"
        MERGE INTO [SummerPractice].[Attendance] AS target
        USING (SELECT @Email AS Email, @LessonId AS Lesson_id, @AttendanceVerify AS Attendance_verify) AS source
        ON (target.Email = source.Email AND target.Lesson_id = source.Lesson_id)
        WHEN MATCHED THEN 
            UPDATE SET Attendance_verify = source.Attendance_verify
        WHEN NOT MATCHED THEN
            INSERT (Email, Lesson_id, Attendance_verify)
            VALUES (source.Email, source.Lesson_id, source.Attendance_verify);";

            var connection = _databaseContext.GetDbConnection();
            var courseId = await connection.QuerySingleOrDefaultAsync<Guid>(
                getCourseIdQuery,
                new { CourseName = courseName },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == default)
            {
                return false;
            }

            var lessonId = await connection.QuerySingleOrDefaultAsync<Guid>(
                getLessonIdQuery,
                new { LessonTitle = lessonTitle, CourseId = courseId },
                _databaseContext.GetDbTransaction()
            );

            if (lessonId == default)
            {
                return false;
            }

            foreach (var student in students)
            {
                
                bool attendanceVerify = true;

                var parameters = new DynamicParameters();
                parameters.Add("Email", student.Email, DbType.String);
                parameters.Add("LessonId", lessonId, DbType.Guid);
                parameters.Add("AttendanceVerify", attendanceVerify ? 1 : 0, DbType.Boolean);

                var result = await connection.ExecuteAsync(insertAttendanceQuery, parameters, _databaseContext.GetDbTransaction());
                if (result == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<List<Attendance>> GetAttendance(string courseName, string lessonTitle)
        {
            var getCourseIdQuery = "SELECT Course_id FROM [SummerPractice].[Courses] WHERE [Name_course] = @CourseName";
            var getLessonIdQuery = "SELECT Lesson_id FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @LessonTitle AND [Course_id] = @CourseId";
            var getAttendanceQuery = @"
                SELECT A.[Email], L.[Lesson_name], A.[Attendance_verify]
                FROM [SummerPractice].[Attendance] A
                INNER JOIN [SummerPractice].[Lessons] L ON A.[Lesson_id] = L.[Lesson_id]
                WHERE L.[Lesson_id] = @LessonId";

            var connection = _databaseContext.GetDbConnection();
            var courseId = await connection.QuerySingleOrDefaultAsync<Guid>(
                getCourseIdQuery,
                new { CourseName = courseName },
                _databaseContext.GetDbTransaction()
            );

            if (courseId == default)
            {
                return new List<Attendance>();
            }

            var lessonId = await connection.QuerySingleOrDefaultAsync<Guid>(
                getLessonIdQuery,
                new { LessonTitle = lessonTitle, CourseId = courseId },
                _databaseContext.GetDbTransaction()
            );

            if (lessonId == default)
            {
                return new List<Attendance>();
            }

            var attendanceRecords = await connection.QueryAsync<Attendance>(
                getAttendanceQuery,
                new { LessonId = lessonId },
                _databaseContext.GetDbTransaction()
            );

            return attendanceRecords.ToList();
        }
    }
}

