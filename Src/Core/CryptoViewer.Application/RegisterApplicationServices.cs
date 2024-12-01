using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoViewer.Application;

public static class RegisterApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(RegisterApplicationServices).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterApplicationServices).Assembly));
        return services;
    }
}
