using RendezVous.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Options;

namespace RendezVous.WebUI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<RendezVousDbContext>();

            try
            {
                if (dbContext.Database.IsSqlServer())
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
            
            var environment = services.GetRequiredService<IWebHostEnvironment>();
            var seedOptions = services.GetRequiredService<IOptions<SeedOptions>>();

            if (environment.IsDevelopment() && seedOptions.Value.Enabled)
            {
                var dateTime = services.GetRequiredService<IDateTime>();

                var seeder = new RendezVousDbContextSeeder(dbContext, seedOptions, dateTime);
                await seeder.Wipe();
                await seeder.Seed();
            }
        }

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.Local.json"))
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}
