using Microsoft.Extensions.DependencyInjection;

using Application.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();
            services.AddScoped<LessonInventoryService>();
            services.AddScoped<LessonService>();
            services.AddScoped<FileService>();
            
            services.AddScoped<CourseInventoryService>();
			services.AddScoped<CourseService>();
			return services;
        }
    }
}
