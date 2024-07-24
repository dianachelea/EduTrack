using Dapper;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure
{
	public static class DapperConfiguration
	{
		public static void DapperConfig(this IServiceCollection services)
		{
			DefaultTypeMap.MatchNamesWithUnderscores = true;
			SqlMapper.SetTypeMap(
				typeof(UserCredentials),
				new CustomPropertyTypeMap(
					typeof(UserCredentials),
					(type, columnName) =>
						type.GetProperties()?.FirstOrDefault(prop =>
							prop.GetCustomAttributes(false)
								.OfType<ColumnAttribute>()
								.Any(predicate: attr => attr.Name == columnName))));
		}
	}
}
