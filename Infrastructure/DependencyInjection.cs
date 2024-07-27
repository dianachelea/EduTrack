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
<<<<<<< HEAD
=======
            services.AddScoped<IGenerateToken, GenerateToken>();
            services.AddScoped<ISendNotification, SendEmailNotification>();
            services.AddScoped<ILinkCreator, LinkCreator>();
            services.AddScoped<IStatistics, StatisticsUtils>();
>>>>>>> zaco

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IGenerateToken, GenerateToken>();
            services.AddScoped<ILinkCreator, LinkCreator>();
            services.AddScoped<ISendNotification, SendEmailNotification>();

            services.AddScoped<ICoursesRepository, CourseRepository>();
            services.AddScoped<IGradesRepository, GradesRepository>();
            services.AddScoped<IAssignmentsRepository, AssignmentsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<ITokenRepository, TokensRepository>();
            return services;
        }
    }
}
