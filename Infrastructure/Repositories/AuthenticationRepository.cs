using Dapper;
using System.Data;

using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class AuthenticationRepository: IAuthenticationRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public AuthenticationRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public Task<IEnumerable<UserCredentials>> GetUser(string email)
        {
            var sql = "SELECT [Username], [Password], [Email], [Role] FROM [SummerPractice].[User] WHERE [Email] = @Email";

            var connection = _databaseContext.GetDbConnection();
            var users = connection.QueryAsync<UserCredentials>(sql, new {Email = email});
            return users;
        }

        public async Task<bool> RegisterUser(UserCredentials credentials)
        {
            var query = "INSERT INTO [SummerPractice].[User] ([Username], [Password], [Email], [Role]) VALUES (@Username, @Password, @Email, @Role)";
            var parameters = new DynamicParameters();
            parameters.Add("Username", credentials.Username, DbType.String);
            parameters.Add("Password", credentials.Password, DbType.String);
            parameters.Add("Email", credentials.Email, DbType.String);
            parameters.Add("Role", credentials.Role, DbType.String);

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
    }
}
