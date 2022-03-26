using RendezVous.Application.Common.Interfaces;
using RendezVous.Infrastructure.Identity;
using RendezVous.Infrastructure.Persistence;
using RendezVous.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RendezVous.Domain.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace RendezVous.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RendezVousDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(RendezVousDbContext).Assembly.FullName)));

        services.AddScoped<IRendezVousDbContext>(provider => provider.GetRequiredService<RendezVousDbContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        var auth0Options = configuration.GetSection("Auth0").Get<Auth0Options>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = auth0Options.Authority;
                options.Audience = auth0Options.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        return services;
    }
}
