using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
	public class TokensRepository : ITokenRepository
	{
		private readonly IDatabaseContext _databaseContext;

		public TokensRepository(IDatabaseContext databaseContext)
		{
			this._databaseContext = databaseContext;
		}

		private Dictionary<string, ValidationTokenDo> tokenRepository = new Dictionary<string, ValidationTokenDo>();

		public async Task<bool> AddToken(ValidationTokenDo token)
		{
			var query = "INSERT INTO [SummerPractice].[Tokens] ([UserEmail], [Token], [ExpirationDate]) VALUES (@UserEmail, @Token, @ExpirationDate)";
			var parameters = new DynamicParameters();
			parameters.Add("UserEmail", token.userEmail, DbType.String);
			parameters.Add("Token", token.token, DbType.String);
			parameters.Add("ExpirationDate", token.expirationDate, DbType.DateTime);

			var connection = _databaseContext.GetDbConnection();
			var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
			return true;
		}

		public Task<IEnumerable<ValidationTokenDo>> GetToken(string token)
		{
			var sql = "SELECT [UserEmail], [Token], [ExpirationDate] FROM [SummerPractice].[Tokens] WHERE [Token] = @Token";

			var connection = _databaseContext.GetDbConnection();
			var validationToken = connection.QueryAsync<ValidationTokenDo>(sql, new { Token = token });
			return validationToken;
		}

		public async Task<bool> DeleteToken(string token)
		{
			var sql = "DELETE FROM [SummerPractice].[Tokens] WHERE [Token] = @Token";

			var connection = _databaseContext.GetDbConnection();
			var validationToken = await connection.ExecuteAsync(sql, new { Token = token }, _databaseContext.GetDbTransaction());
			return validationToken != 0;
		}
	}
}
