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

        public async Task<bool> EditAssignment(string coursename, string lessontitle, AssignmentDo content, string FileName)
        {
            var querySelectOldFile = @"
        SELECT [Assignment_file]
        FROM [SummerPractice].[Lessons]
        WHERE [Lesson_name] = @lessontitle
          AND [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @coursename);";

            var queryUpdateLesson = @"
        UPDATE [SummerPractice].[Lessons]
        SET [Assignment_name] = @Name,
            [Assignment_description] = @Description,
            [Assignment_file] = @File,
            [Assignment_preview] = @Preview
        WHERE [Lesson_name] = @lessontitle
          AND [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @coursename);";

            var queryDeleteOldFile = @"
        DELETE FROM [SummerPractice].[file]
        WHERE [FileName] = @file_name;";

            var parameters = new DynamicParameters();
            parameters.Add("Name", content.Assignment_name, DbType.String);
            parameters.Add("Description", content.Assignment_description, DbType.String);
            parameters.Add("File", FileName, DbType.String);
            parameters.Add("Preview", content.Assignment_preview, DbType.String);
            parameters.Add("lessontitle", lessontitle, DbType.String);
            parameters.Add("coursename", coursename, DbType.String);

            using var connection = _databaseContext.GetDbConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                var oldFileName = await connection.QuerySingleOrDefaultAsync<string>(querySelectOldFile, parameters, transaction);

                var updateResult = await connection.ExecuteAsync(queryUpdateLesson, parameters, transaction);

                if (!string.IsNullOrEmpty(oldFileName) && oldFileName != FileName)
                {
                    parameters.Add("file_name", oldFileName, DbType.String);
                    await connection.ExecuteAsync(queryDeleteOldFile, parameters, transaction);
                }

                transaction.Commit();

                if (!string.IsNullOrEmpty(oldFileName) && oldFileName != FileName)
                {
                    string parentDirectory = GetParentDirectoryOfCurrent();
                    if (!string.IsNullOrEmpty(parentDirectory))
                    {
                        string filesDirectory = Path.Combine(parentDirectory, "Domain", "files");

                        if (Directory.Exists(filesDirectory))
                        {
                            var files = Directory.GetFiles(filesDirectory, "*", SearchOption.AllDirectories)
                                                 .ToList();

                            foreach (var file in files)
                            {
                                if (Path.GetFileName(file).Equals(oldFileName, StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        File.Delete(file);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                }

                return updateResult != 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }


        public async Task<bool> DeleteAssignment(string coursename, string lessontitle)
        {
            var queryAux1 = @"
                SELECT [FileName]
                FROM [SummerPractice].[Grade]
                WHERE [Lesson_id] = (SELECT [Lesson_id] FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @lessontitle);";

            var queryAux2 = @"
                SELECT [Assignment_file]
                FROM [SummerPractice].[Lessons]
                WHERE [Lesson_name] = @lessontitle;";

            var queryUpdateLesson = @"
                UPDATE [SummerPractice].[Lessons]
                SET [Assignment_name] = NULL,
                    [Assignment_description] = NULL,
                    [Assignment_file] = NULL,
                    [Assignment_preview] = NULL
                WHERE [Lesson_name] = @lessontitle
                  AND [Course_id] = (SELECT [Course_id] FROM [SummerPractice].[Courses] WHERE [Name_course] = @coursename);";

            var queryDeleteGrade = @"
                DELETE FROM [SummerPractice].[Grade]
                WHERE [Lesson_id] = (SELECT [Lesson_id] FROM [SummerPractice].[Lessons] WHERE [Lesson_name] = @lessontitle);";

            var queryDeleteFile1 = @"
                DELETE FROM [SummerPractice].[file]
                WHERE [FileName] = @file_name1;";

            var queryDeleteFile2 = @"
                DELETE FROM [SummerPractice].[file]
                WHERE [FileName] = @file_name2;";

            var parameters = new DynamicParameters();
            parameters.Add("lessontitle", lessontitle, DbType.String);
            parameters.Add("coursename", coursename, DbType.String);

            using var connection = _databaseContext.GetDbConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                var fileName1 = await connection.QuerySingleOrDefaultAsync<string>(queryAux1, parameters, transaction);
                var fileName2 = await connection.QuerySingleOrDefaultAsync<string>(queryAux2, parameters, transaction);

                parameters.Add("file_name1", fileName1, DbType.String);
                parameters.Add("file_name2", fileName2, DbType.String);

                var updateResult = await connection.ExecuteAsync(queryUpdateLesson, parameters, transaction);

                await connection.ExecuteAsync(queryDeleteGrade, parameters, transaction);
                await connection.ExecuteAsync(queryDeleteFile1, parameters, transaction);
                await connection.ExecuteAsync(queryDeleteFile2, parameters, transaction);

                transaction.Commit();

                string parentDirectory = GetParentDirectoryOfCurrent();
                if (!string.IsNullOrEmpty(parentDirectory))
                {
                    string filesDirectory = Path.Combine(parentDirectory, "Domain", "files");

                    if (Directory.Exists(filesDirectory))
                    {
                        var files = Directory.GetFiles(filesDirectory, "*", SearchOption.AllDirectories)
                                             .ToList();

                        foreach (var file in files)
                        {
                            string fileName = Path.GetFileName(file);

                            if (fileName.Equals(fileName1, StringComparison.OrdinalIgnoreCase) ||
                                fileName.Equals(fileName2, StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    File.Delete(file);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                                }
                            }
                        }
                    }
                }

                return updateResult != 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
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

        private string GetParentDirectoryOfCurrent()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            var parentDirectoryInfo = Directory.GetParent(currentDirectory);

            return parentDirectoryInfo?.FullName;
        }

    }
}
