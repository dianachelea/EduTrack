﻿using Microsoft.Extensions.DependencyInjection;

using Application.Interfaces;
using Infrastructure.Handlers;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
<<<<<<< HEAD
using Microsoft.AspNetCore.Diagnostics;
=======
using Infrastructure.Utils;
>>>>>>> main

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
<<<<<<< HEAD
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
=======
            services.AddScoped<ITokenRepository, TokensRepository>();
>>>>>>> main

            return services;
        }
    }
}
