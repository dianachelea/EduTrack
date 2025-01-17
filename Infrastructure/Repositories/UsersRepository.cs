﻿using Dapper;
using System.Data;

using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public UsersRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public Task<IEnumerable<UserInfo>> GetUserInfo(string email)
        {
            var sql = "SELECT [Username], [Email], [Role], [First_name], [Last_Name], [Phone_number] FROM [SummerPractice].[User] WHERE [Email] = @Email";

            var connection = _databaseContext.GetDbConnection();
            var users = connection.QueryAsync<UserInfo>(sql, new {Email = email});
            return users;
        }

        public async Task<bool> RegisterUser(UserCredentials credentials)
        {
			var query = "INSERT INTO [SummerPractice].[User] ([Username], [Password], [Email], [Role], [Phone_number], [First_name], [Last_Name]) VALUES (@Username, @Password, @Email, @Role, @Phone, @FirstName, @LastName)";
			var parameters = new DynamicParameters();
			parameters.Add("Username", credentials.Username, DbType.String);
			parameters.Add("Password", credentials.Password, DbType.String);
			parameters.Add("Email", credentials.Email, DbType.String);
			parameters.Add("Role", credentials.Role, DbType.String);
			parameters.Add("Phone", credentials.Phone, DbType.String);
			parameters.Add("FirstName", credentials.FirstName, DbType.String);
			parameters.Add("LastName", credentials.LastName, DbType.String);

			var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<bool> GiveUserAdminRights(string email)
        {
            var sql = "UPDATE [SummerPractice].[User] SET [Role] = 'admin' WHERE [Email] = @Email";

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(sql, new { Email = email });
            return result != 0;
        }
        public async Task<bool> UpdatePassword(string email, string newPassword)
        {
            var sql = "UPDATE [SummerPractice].[User] SET [Password] = @Password WHERE [Email] = @Email";

            var connection = _databaseContext.GetDbConnection();

			var parameters = new DynamicParameters();
			parameters.Add("Password", newPassword, DbType.String);
			parameters.Add("Email", email, DbType.String);

			var result = await connection.ExecuteAsync(sql, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

		public Task<IEnumerable<UserCredentials>> GetAllStudents()
		{
			var sql = "SELECT [Username], [Email], [Role], [First_name], [Last_Name], [Phone_number] FROM [SummerPractice].[User]";
            
			var connection = _databaseContext.GetDbConnection();
			var users = connection.QueryAsync<UserCredentials>(sql);
			return users;
		}
    }
}
