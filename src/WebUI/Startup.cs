using RendezVous.Application;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Infrastructure;
using RendezVous.WebUI.Filters;
using RendezVous.WebUI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace RendezVous.WebUI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddApplication()
            .AddInfrastructure(Configuration);
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddControllers(opt =>
        {
            opt.Filters.Add<RendezVousExceptionFilterAttribute>();
        }).AddFluentValidation(x => x.AutomaticValidationEnabled = false);
        services.AddRazorPages();


        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument(opt =>
        {
            opt.Title = "RendezVous API";
            opt.Version = "v1";
        });

        services.AddHttpContextAccessor();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
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

        if (env.IsDevelopment())
        {
            app.UseSwaggerUi3(opt =>
            {
                opt.Path = "/swagger";
                opt.DocumentPath = "/api/specification.json";
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                spa.Options.DevServerPort = 3000;

                spa.UseReactDevelopmentServer(npmScript: "start-for-dotnet");
            });
        }
    }
}