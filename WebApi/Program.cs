using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

using Application;
using Infrastructure;
using Infrastructure.Handlers;
using Microsoft.IdentityModel.Tokens;
using WebApiContracts;
using Infrastructure.Utils;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.WithOrigins("http://localhost:4200")
						   .AllowAnyHeader()
						   .AllowAnyMethod();
				});
			});
			

			builder.Services.AddExceptionHandler<NullReferenceErrorHandler>();
            builder.Services.AddExceptionHandler<GenericErrorHandler>();

			builder.Services.DapperConfig();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddScheme<JwtBearerOptions, JwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config.GetSection("JwtSettings:Issuer").Value!,
                    ValidAudience = config.GetSection("JwtSettings:Audience").Value!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JwtSettings:SecurityKey").Value!)),
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityData.AdminUserPolicyName, p =>
                    p.RequireClaim(IdentityData.AdminUserClaimName, "true"));
                options.AddPolicy(IdentityData.TeacherUserPolicyName, p =>
                    p.RequireClaim(IdentityData.TeacherUserClaimName, "true"));
            });

            builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseExceptionHandler(_ => { });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

			app.UseCors();

			app.MapControllers();

            app.Run();
        }
    }
}