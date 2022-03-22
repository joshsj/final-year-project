using RendezVouz.Repositories.Common;
using RendezVouz.Services;
using RendezVouz.Services.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Mapster;
using RendezVouz.Domain.Options;
using RendezVouz.Controllers.Common.Filters;

namespace RendezVouz;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var isDevelopment = builder.Environment.IsDevelopment();

        var configuration = GetOptions<ConfigurationOptions>(builder);

        builder.Services.AddDbContextPool<RendezVousDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.DatabaseConnectionString);
        });

        builder.Services.AddControllers(opt =>
        {
            opt.Filters.Add<RendezVousExceptionFilterAttribute>();
        });
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        ConfigureOptions(builder);
        ConfigureRepositoryDependencies(builder);
        ConfigureServiceDependencies(builder);

        if (isDevelopment)
        {
            ConfigureTestDependencies(builder);
        }

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        if (isDevelopment)
        {
            app.UseSwagger().UseSwaggerUI();
        }

        TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
        TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;

        app.Run();
    }

    private static void ConfigureRepositoryDependencies(WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<RendezVousDbContext>();
    }

    private static void ConfigureServiceDependencies(WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<IDateTimeService, DateTimeService>();
    }

    private static void ConfigureTestDependencies(WebApplicationBuilder builder)
    {
        builder.Logging.AddConsole();
    }

    private static void ConfigureOptions(WebApplicationBuilder builder)
    {
        void ConfigureOptions<T>() where T : class
        {
            var options = GetOptions<T>(builder);

            if (options is null) { return; }

            builder.Services.AddSingleton(options);
        }

        ConfigureOptions<ConfigurationOptions>();
    }

    private static T GetOptions<T>(WebApplicationBuilder builder) where T : class
    {
        return builder.Configuration
            .GetSection(typeof(T).Name.Replace("Options", ""))
            .Get<T>();
    }
}