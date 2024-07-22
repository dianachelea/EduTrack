using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AssignmentsRepository: IAssignmentsRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public AssignmentsRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public Task<IEnumerable<AssignmentDo>> GetAssignment(string coursename, string lessontitle)
        {
            var sql = "SELECT [Assignment_name], [Assignment_description], [Assignment_file], [Assignment_preview] FROM [SummerPractice].[Lessons] " +
                "WHERE [Lesson_name] = @lessontitle AND [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @coursename); ";

            var connection = _databaseContext.GetDbConnection();
            var assignment = connection.QueryAsync<AssignmentDo>(sql, new { CourseName = coursename,lessonTitle = lessontitle });
            return assignment;
        }


        public async Task<bool> AddAssignment(string coursename,string lessontitle,AssignmentDo assignmentData, string FileName)
        {
            var query = "UPDATE [SummerPractice].[Lessons]" +
            "SET"+
                "[Assignment_name] = @Name," +
                "[Assignment_description] = @Description,"+
                "[Assignment_file] = @File,"+
                "[Assignment_preview] = @Preview "+
            "WHERE"+
                "[Lesson_name] = @lessontitle "+
                "AND [Course_id] = (SELECT[Course_id] FROM[SummerPractice].[Courses] WHERE [Name_course] = @coursename);";
            var parameters = new DynamicParameters();
            parameters.Add("Name", assignmentData.Assignment_name, DbType.String);
            parameters.Add("Description", assignmentData.Assignment_description, DbType.String);
            parameters.Add("File", FileName, DbType.String);
            parameters.Add("Preview", assignmentData.Assignment_preview, DbType.String);
            parameters.Add("lessontitle", lessontitle, DbType.String);
            parameters.Add("coursename", coursename, DbType.String);


            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<IEnumerable<List<AssignmentDo>>> GetStudentAssignments(string coursename, string studentemail)
        {
            var sql = @"
                SELECT 
                    [Assignment_name] AS AssignmentName,
                    [Assignment_description] AS AssignmentDesc,
                    [Assignment_preview] AS AssignmentPrev,
                    [Assignment_file] AS AssignmentFile
                FROM [SummerPractice].[Lessons]
                WHERE [Course_id] = (
                    SELECT [Course_id] 
                    FROM [SummerPractice].[Courses] 
                    WHERE [Name_course] = @courseName 
                )
                AND [Course_id] = (
                    SELECT [Course_id] 
                    FROM [SummerPractice].[Students-Courses] 
                    WHERE [Email] = @studentEmail 
                )";


            var connection = _databaseContext.GetDbConnection();

            var data = await connection.QueryAsync(
                sql,
                new { courseName = coursename, studentEmail = studentemail }
            );

            var assignments = data
                .Select(row => new AssignmentDo
                {
                    Assignment_name = (string)row.AssignmentName,
                    Assignment_description = (string)row.AssignmentDesc,
                    Assignment_file = (string)row.AssignmentFile,
                    Assignment_preview = (string)row.AssignmentPrev
                })
                .ToList();

            var groupedAssignments = assignments
                .GroupBy(a => a.Assignment_name)
                .Select(g => g.ToList())
                .ToList();

            return groupedAssignments;
        }



    }
}
