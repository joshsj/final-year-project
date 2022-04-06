using RendezVous.Application.Common.Interfaces;
using RendezVous.Infrastructure.Persistence;
using RendezVous.WebUI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using RendezVous.Domain.Common;
using RendezVous.Domain.Entities;
using Respawn;

namespace RendezVous.Application.IntegrationTests;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration = null!;

    private static IServiceScopeFactory _scopeFactory = null!;

    private static Checkpoint _checkpoint = null!;

    private static string? _currentUserProviderId;
    private static readonly Random _random = new ();
    
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "RendezVous.WebUI"));

        startup.ConfigureServices(services);

        // Replace current user service with testing version
        services.RemoveAll<ICurrentUserService>();
        services.AddTransient(_ => Mock.Of<ICurrentUserService>(s => s.ProviderId == _currentUserProviderId));

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
        
        // Prevent rerun of migrations
        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        };

        await EnsureDatabase();
    }

    private static async Task EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RendezVousDbContext>();

        await context.Database.MigrateAsync();
    }
    
    public static async Task ResetState()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));

        _currentUserProviderId = null;
    }

    public static string CreateProviderId() => $"testing-oauth2:{_random.Next(int.MaxValue / 100, int.MaxValue)}";
    public static async Task<(string ProviderId, Guid Id)> RunAsUser()
    {
        // mimics an Auth0 user_id
        _currentUserProviderId = CreateProviderId();

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Test Employee",
            Email = "testEmployee@email.com",
            ProviderId = _currentUserProviderId
        };

        await Add(employee);

        return (_currentUserProviderId, employee.Id);
    }

    public static async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task<T?> Find<T>(params object[] keyValues) where T : Entity
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RendezVousDbContext>();

        return await context.FindAsync<T>(keyValues);
    }

    public static async Task Add<T>(params T[] entities) where T : Entity
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RendezVousDbContext>();

        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public static async Task<int> Count<T>() where T : Entity
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RendezVousDbContext>();

        return await context.Set<T>().CountAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
