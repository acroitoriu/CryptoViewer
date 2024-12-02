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
        services.AddOptions<ExchangeServiceSettings>()
            .BindConfiguration("ExchangeServiceSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddHttpClient<IExchangeService, ExchangeRatesApiHttpClient>(cfg => {
            cfg.BaseAddress = new Uri(configuration.GetValue<string>("ExchangeServiceSettings:BaseUrl"));
        });                
        return services;
    }

}
