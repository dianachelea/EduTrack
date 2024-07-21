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
    public class GradesRepository : IGradesRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public GradesRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public async Task<bool> SolveAssignment(string courseName, string lessonTitle, string studentEmail, AssignmentSolutionDo solution)
        {

            var query = "INSERT INTO [SummerPractice].[Grade] ([Email], [Solution], [Solution_title], [Lesson_id], [FileName], [Grade]) "+
            "SELECT @Email,@Solution, @SolutionTitle, l.lesson_id, @FileName, @Grade " +
            "FROM [SummerPractice].[Lessons] l "+
            "JOIN [SummerPractice].[Courses] c ON l.course_id = c.course_id "+
            "WHERE c.Name_course = @courseName AND l.Lesson_name = @lessonTitle ";
            var parameters = new DynamicParameters();
            parameters.Add("Email", studentEmail, DbType.String);
            parameters.Add("courseName", courseName, DbType.String);
            parameters.Add("lessonTitle", lessonTitle, DbType.String);
            parameters.Add("Solution", solution.Solution, DbType.String);
            parameters.Add("SolutionTitle", solution.Solution_title, DbType.String);
            parameters.Add("FileName",solution.FileName, DbType.String);
            parameters.Add("Grade", 0, DbType.Int64);

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public Task<IEnumerable<AssignmentSolutionDo>> GetStudentSolution(string coursename, string lessontitle, string studentemail)
        {
            var sql = "SELECT [Email], [Solution], [Solution_title], [Lesson_id], [FileName], [Grade] FROM [SummerPractice].[Grade] " +
                "WHERE [Lesson_id] =  (SELECT [Lesson_id] FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @lessonTitle " +
                "AND [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @courseName)) " +
                "AND [Email] = @Email; ";

            var connection = _databaseContext.GetDbConnection();
            var grade = connection.QueryAsync<AssignmentSolutionDo>(sql, new { courseName = coursename,lessonTitle = lessontitle, Email = studentemail});
            return grade;
        }

        public async Task<IEnumerable<List<AssignmentGradeDo>>> GetAllAssignmentsSent(string coursename, string lessontitle)
        {
            var sql = @"
        SELECT 
            u.[First_name] AS StudentFirstName,
            u.[Last_Name] AS StudentLastName,
            g.[Email],
            g.[Solution],
            g.[Solution_title] AS SolutionTitle,
            g.[Lesson_id] AS LessonId,
            g.[FileName],
            g.[Grade]
        FROM [SummerPractice].[Grade] g
        JOIN [SummerPractice].[User] u ON g.[Email] = u.[Email]
        WHERE g.[Lesson_id] = (
            SELECT [Lesson_id] 
            FROM [SummerPractice].[Lessons] 
            WHERE [Lesson_name] = @lessonTitle 
            AND [Course_id] = (
                SELECT [Course_id] 
                FROM [SummerPractice].[Courses] 
                WHERE [Name_course] = @courseName
            )
        )";

            var connection = _databaseContext.GetDbConnection();

            var data = await connection.QueryAsync(
                sql,
                new { courseName = coursename, lessonTitle = lessontitle }
            );

            var grades = data
                .Select(row => new AssignmentGradeDo
                {
                    Student = new StudentDo
                    {
                        First_name = (string)row.StudentFirstName,
                        Last_Name = (string)row.StudentLastName,
                        Email = (string)row.Email
                    },
                    Lesson_name = lessontitle,
                    Grade = (double)row.Grade
                })
                .ToList();

            var groupedGrades = grades
                .GroupBy(g => g.Grade)
                .Select(g => g.ToList())
                .ToList();

            return groupedGrades;
        }

        public async Task<bool> GradeAssignment(string coursename, string lessontitle, double grade, string studentemail)
        {
            var query = "UPDATE [SummerPractice].[Grade]" +
            "SET" +
                "[Grade] = @Grade " +
            "WHERE" +
                "[Lesson_id] = (SELECT [Lesson_id] FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @lessonTitle AND [Course_id] = (SELECT [Course_id] FROM[SummerPractice].[Courses] WHERE [Name_course] = @courseName)) " +
                "AND [Email] = @studentEmail ";
            var parameters = new DynamicParameters();
            parameters.Add("courseName", coursename, DbType.String);
            parameters.Add("lessonTitle", lessontitle, DbType.String);
            parameters.Add("Grade", grade, DbType.Double);
            parameters.Add("studentEmail", studentemail, DbType.String);


            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public Task<IEnumerable<double>> GetGrade(string coursename, string lessontitle, string studentemail)
        {
            var sql = "SELECT [Grade] FROM [SummerPractice].[Grade] " +
                "WHERE [Lesson_id] =  (SELECT [Lesson_id] FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @lessonTitle AND [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @courseName)) " +
                "AND [Email] = @Email; ";

            var connection = _databaseContext.GetDbConnection();
            var grade = connection.QueryAsync<double>(sql, new { courseName = coursename, lessonTitle = lessontitle, Email = studentemail });
            return grade;
        }

    }
}
