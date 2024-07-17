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


        public async Task<bool> AddAssignment(string coursename,string lessontitle,AssignmentDo assignmentData)
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
            parameters.Add("File", assignmentData.Assignment_file, DbType.String);
            parameters.Add("Preview", assignmentData.Assignment_preview, DbType.String);
            parameters.Add("lessontitle", lessontitle, DbType.String);
            parameters.Add("coursename", coursename, DbType.String);


            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        
    }
}
