﻿using Dapper;

using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;
using System.Data;
using Domain.Exceptions;

namespace Infrastructure.Repositories
{
    public class FileRepository: IFileRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public FileRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public Task<IEnumerable<FileDetails>> GetFile(string fileName)
        {
            var sql = "SELECT [FileName], [Path] FROM [SummerPractice].[File] WHERE [FileName] = @FileName";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.QueryAsync<FileDetails>(sql, new { FileName = fileName });
            return file;
        }

        public async Task<bool> SaveFile(string fileName, string fileLocation)
        {
            var checkDuplicate = await this.GetFile(fileName);
            if (checkDuplicate.ToList().Count > 0)
            {
                throw new DuplicateFileException(fileName);
            }

            var query = "INSERT INTO [SummerPractice].[File] ([FileName], [Path]) VALUES (@FileName, @Path)";
            var parameters = new DynamicParameters();
            parameters.Add("FileName", fileName, DbType.String);
            parameters.Add("Path", fileLocation, DbType.String);

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }
    }
}
