using CryptoViewer.Infrastructure.Configuration;
using CryptoViewer.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace CryptoViewer.Infrastructure.UnitTests;

public class ExchangeRatesApiHttpClientUnitTests
{
    private readonly Mock<ILogger<ExchangeRatesApiHttpClient>> _loggerMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<IHttpClientFactory> _httpFactoryMock;

    public ExchangeRatesApiHttpClientUnitTests()
    {
        _loggerMock = new Mock<ILogger<ExchangeRatesApiHttpClient>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _httpClient.BaseAddress = new Uri("http://nonexisting.domain");        
    }

    
    [Fact]
    public async Task GivenValidInput_WhenCallingGetCurrencyQuotes_ThenSuccessfulResponseWithListOfQuotes()
    {
        var settings = new ExchangeServiceSettings
        {
            BaseUrl = "https://localhost",
            AccessKey = "super_secret_key"
        };        
        const string testContent = "{\"success\": true,\"timestamp\": 1733056755,\"base\": \"EUR\",\"date\": \"2024-12-01\",\"rates\": {\"AED\": 3.885787,\"AFN\": 71.80765,\"ALL\": 98.239422,\"AMD\": 418.169843,\"BTC\": 1.090958e-5}}";        
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(testContent)
            });                              
        var apiClient = new ExchangeRatesApiHttpClient(_httpClient, _loggerMock.Object, Options.Create<ExchangeServiceSettings>(settings));
        var response =  await apiClient.GetCurrencyQuotes("BTC", CancellationToken.None);
        response.Should().NotBeEmpty();
        response.Should().HaveCount(5);
    }

    [Fact]
    public async Task GivenApiErrorResponse_WhenCallingGetCurrencyQuotes_ThenEmptyListAndLoggerMessage()
    {
        var settings = new ExchangeServiceSettings
        {
            BaseUrl = "https://localhost",
            AccessKey = "super_secret_key"
        };
        const string testContent = "{\"error\": {\"code\": \"invalid_base_currency\",\"message\": \"An unexpected error ocurred. [Technical Support: support@apilayer.com]\"}}";
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(testContent)
            });
        var apiClient = new ExchangeRatesApiHttpClient(_httpClient, _loggerMock.Object, Options.Create<ExchangeServiceSettings>(settings));
        var response = await apiClient.GetCurrencyQuotes("BTC", CancellationToken.None);
        _loggerMock.Verify(l =>
            l.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("The call to get the latest rates failed.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), Times.Once);
        response.Should().BeEmpty();        
    }    
}
