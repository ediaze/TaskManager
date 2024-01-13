using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Interfaces;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection? AddApplicationServices(this IServiceCollection services, IConfigurationManager config)
        {
            string secretKey = Environment.GetEnvironmentVariable("Authentication_TokenKey")
            ?? throw new ArgumentException("Authentication_TokenKey");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            AddDatabaseServices(services, config);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = new List<CultureInfo> { new("en-US") };
                options.SupportedUICultures = new List<CultureInfo> { new("en-US") };
            });

            services.AddSingleton(config);

            // A new instance is created once per client request (connection).
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            // A new instance is created every time a service is requested.
            services.AddTransient<ITaskItemRepository, TaskItemRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

        private static void AddDatabaseServices(IServiceCollection services, IConfigurationManager config)
        {
            const string databaseName = "TaskManagerDatabase";
            const string databaseConnectionString = "ConnectionStrings__TaskManagerDatabase";

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var connectionString = !string.IsNullOrEmpty(config.GetConnectionString(databaseName))
                ? config.GetConnectionString(databaseName)
                : !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(databaseConnectionString))
                ? Environment.GetEnvironmentVariable(databaseConnectionString)
                : throw new InvalidOperationException("Connection string not found in environment variables.");

            services.AddDbContext<TaskManagerDbContext>(options =>
            {
                switch (environmentName)
                {
                    case "InMemory":
                        options.UseInMemoryDatabase(connectionString ?? environmentName);
                        break;
                    default:
                        options.UseSqlServer(connectionString);
                        break;
                }
            });
        }

        public static ILoggingBuilder AddLogging(this ILoggingBuilder logging)
        {
            // Configure logging
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            // You can also add third-party providers like Serilog, NLog, etc.

            return logging;
        }
    }
}
