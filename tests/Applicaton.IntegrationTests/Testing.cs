using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.WebUI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[SetUpFixture]
public class Testing
{   
    private static IConfigurationRoot configuration;
    private static IWebHostEnvironment environment;
    private static IServiceScopeFactory scopeFactory;
    private static Checkpoint checkpoint;
    private static string currentUserId;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        configuration = builder.Build();
        environment = Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "CleanArchitecture.WebUI");

        var startup = new Startup(configuration, environment);

        var services = new ServiceCollection();

        services.AddSingleton(environment);

        services.AddLogging();

        startup.ConfigureServices(services);

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        services.Remove(currentUserServiceDescriptor);

        // Register testing version
        services.AddTransient(provider =>
            Mock.Of<ICurrentUserService>(s => s.UserId == currentUserId));

        scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        
        checkpoint = new Checkpoint
        {
            TablesToIgnore = new [] { "__EFMigrationsHistory" }
        };

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<IMediator>();

        return await mediator.Send(request);
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!");
    }

    public static async Task<string> RunAsUserAsync(string userName, string password)
    {
        using var scope = scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser { UserName = userName, Email = userName };

        var result = await userManager.CreateAsync(user, password);

        currentUserId = user.Id;

        return currentUserId;
    }

    public static async Task ResetState()
    {
        await checkpoint.Reset(configuration.GetConnectionString("DefaultConnection"));
        currentUserId = null;
    }

    public static async Task<TEntity> FindAsync<TEntity>(int id)
        where TEntity : class
    {
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(id);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
