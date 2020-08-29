using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.DapperPersistence;
using CleanArchitecture.Infrastructure.DapperPersistence.Database;
using CleanArchitecture.Infrastructure.DapperPersistence.Identity.Models;

namespace CleanArchitecture.WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var connectionString = services.GetRequiredService<ConnectionString>();
                    var migrationsPath = services.GetRequiredService<MigrationsPath>();
                    
                    new DatabaseMigration(connectionString, migrationsPath).Execute();

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                    await ApplicationSeeder.SeedDefaultUserAsync(userManager);
                    await ApplicationSeeder.SeedSampleDataAsync(connectionString);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
