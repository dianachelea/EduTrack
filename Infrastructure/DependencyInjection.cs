using Microsoft.Extensions.DependencyInjection;

using Application.Interfaces;
using Infrastructure.Handlers;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Utils;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, DatabaseContext>();

            services.AddScoped<IIdentityHandler, IdentityHandler>();
            services.AddScoped<IPasswordHasher, PasswordHandler>();
            services.AddScoped<IGenerateToken, GenerateToken>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddSingleton<ITokenRepository, TokensRepository>();
            return services;
        }
    }
}
