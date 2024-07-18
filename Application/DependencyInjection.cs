using Microsoft.Extensions.DependencyInjection;

using Application.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
          
            services.AddScoped<CourseInventoryService>();
			services.AddScoped<CourseService>();
			services.AddScoped<AuthorizationService>();
			services.AddScoped<UserService>();
			services.AddScoped<FileService>();
			services.AddScoped<NotificationService>();
			return services;
        }
    }
}
