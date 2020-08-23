using System.IO;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.DapperPersistence;
using CleanArchitecture.Infrastructure.DapperPersistence.Database;
using CleanArchitecture.Infrastructure.Files;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

                services.AddDefaultIdentity<ApplicationUser>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
            
            var connectionString = new ConnectionString(configuration.GetConnectionString("DefaultConnection"));
            services.AddSingleton(connectionString);
            
            MigrationsPath migrationsPath = new MigrationsPath(Path.Combine(environment.ContentRootPath, 
                "../Infrastructure/Persistence/SQL/Migrations/"));
            services.AddSingleton(migrationsPath);
            
            new DatabaseMigration(environment, connectionString, migrationsPath).Execute();
            
            return services;
        }
    }
}
