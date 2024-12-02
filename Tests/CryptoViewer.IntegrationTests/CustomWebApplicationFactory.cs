using CryptoViewer.Application.Contracts;
using CryptoViewer.Infrastructure.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CryptoViewer.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    public Mock<IExchangeService> _exchangeServiceMock = new Mock<IExchangeService>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //base.ConfigureWebHost(builder);
        builder.UseEnvironment("IntegrationTests");
        builder.ConfigureServices(services => {
            services.AddSingleton(_exchangeServiceMock.Object);                        
        });
    }
}
