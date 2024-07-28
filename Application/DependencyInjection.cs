using Microsoft.Extensions.DependencyInjection;

using Application.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();
            services.AddScoped<UserService>();
            services.AddScoped<LessonInventoryService>();
            services.AddScoped<LessonService>();
            services.AddScoped<FileService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<FeedbackService>();
            services.AddScoped<AssignmentInventoryService>();
            services.AddScoped<AssignmentService>();
            
            services.AddScoped<CourseInventoryService>();
			services.AddScoped<CourseService>();
            services.AddScoped<StatisticsService>();

			return services;
        }
    }
}
