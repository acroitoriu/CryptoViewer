using CryptoViewer.Application;
using CryptoViewer.Application.Contracts;
using CryptoViewer.Infrastructure.Configuration;
using CryptoViewer.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoViewer.Infrastructure;

public static class RegisterInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ExchangeServiceSettings>(configuration.GetSection("ExchangeServiceSettings"));
        services.AddHttpClient<IExchangeService, ExchangeRatesApiHttpClient>("exchangeRatesApi", cfg => { 
            // set base address
        });                
        return services;
    }

}
