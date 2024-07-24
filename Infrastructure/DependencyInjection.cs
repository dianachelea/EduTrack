using Microsoft.Extensions.DependencyInjection;

using Application.Interfaces;
using Infrastructure.Handlers;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
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
            services.AddScoped<ISendNotification, SendEmailNotification>();
            services.AddScoped<ILinkCreator, LinkCreator>();

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<ITokenRepository, TokensRepository>();

            return services;
        }
    }
}
