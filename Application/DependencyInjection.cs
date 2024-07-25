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
			services.AddScoped<FileService>();
			services.AddScoped<NotificationService>();
            services.AddScoped<FeedbackService>();
            services.AddScoped<StatisticsService>();

			return services;
        }
    }
}
