using RendezVous.Repositories.Common;
using RendezVous.Services;
using RendezVous.Services.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mapster;
using RendezVous.Domain.Options;
using RendezVous.Controllers.Common.Filters;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

var configuration = GetOptions<ConfigurationOptions>(builder);

builder.Services.AddDbContextPool<RendezVousDbContext>(opt =>
{
    // https://github.com/dotnet/efcore/issues/21361
    opt.UseSqlServer(configuration.DatabaseConnectionString);
});

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<RendezVousExceptionFilterAttribute>();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(opt =>
{
    opt.Title = "RendezVous API";
    opt.Version = "v1";
});
builder.Services.AddRazorPages();

// options
ConfigureOptions<ConfigurationOptions>(builder);

// repositories
builder.Services
    .AddTransient<RendezVousDbContext>();

// services
builder.Services
    .AddTransient<IDateTimeService, DateTimeService>();

if (isDevelopment)
{
    builder.Logging.AddConsole();
}

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

if (isDevelopment)
{
    app.UseSwaggerUi3(opt =>
    {
        opt.DocumentPath = "openApiSpecification.json";
    });

    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp";
        spa.Options.DevServerPort = 3000;
        spa.Options.StartupTimeout = TimeSpan.FromMinutes(2);

        spa.UseReactDevelopmentServer(npmScript: "start-for-dotnet");
    });

    using var scope = app.Services.CreateAsyncScope();
    scope.ServiceProvider
        .GetRequiredService<RendezVousDbContext>()
        .Database
        .Migrate();
}

TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;

app.Run();

static void ConfigureOptions<T>(WebApplicationBuilder builder) where T : class
{
    var options = GetOptions<T>(builder);

    if (options is null) { return; }

    builder.Services.AddSingleton(options);
}

static T GetOptions<T>(WebApplicationBuilder builder) where T : class
{
    return builder.Configuration
        .GetSection(typeof(T).Name.Replace("Options", ""))
        .Get<T>();
}