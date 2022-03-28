using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RendezVous.Domain.Options;

namespace RendezVous.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        void ConfigureOptions<T>() where T : class => 
            services.Configure<T>(configuration.GetSection(typeof(T).Name.Replace("Options", "")));

        ConfigureOptions<Auth0Options>();
        
        return services;
    }
    
    
}
