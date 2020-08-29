using System.IO;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Stores;
using CleanArchitecture.Infrastructure.DapperPersistence;
using CleanArchitecture.Infrastructure.DapperPersistence.Database;
using CleanArchitecture.Infrastructure.DapperPersistence.Identity;
using CleanArchitecture.Infrastructure.DapperPersistence.Identity.Models;
using CleanArchitecture.Infrastructure.DapperPersistence.Identity.Stores;
using CleanArchitecture.Infrastructure.DapperPersistence.Stores;
using CleanArchitecture.Infrastructure.Files;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApplicationUser = CleanArchitecture.Infrastructure.DapperPersistence.Identity.Models.ApplicationUser;

namespace CleanArchitecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
 
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();
            
            // services.AddIdentityServer()
            //     .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
            
            var connectionString = new ConnectionString(configuration.GetConnectionString("DefaultConnection"));
            services.AddSingleton(connectionString);

            services.AddScoped<IGetDbConnection, SqlConnectionGetter>();
            
            MigrationsPath migrationsPath = new MigrationsPath(Path.Combine(environment.ContentRootPath, 
                "../Infrastructure/Persistence/SQL/Migrations/"));
            services.AddSingleton(migrationsPath);

            services.AddSingleton<IMigrateDatabase, DatabaseMigration>();

            services.AddScoped<ITodoListStore, TodoListStore>();
            services.AddScoped<ITodoItemStore, TodoItemStore>();
            
            return services;
        }
    }
}
