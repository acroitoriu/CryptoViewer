using CryptoViewer.Domain.Entities;
using FluentAssertions;
using Moq;

namespace CryptoViewer.IntegrationTests;

public class CurrencyControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;    

    public CurrencyControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client= factory.CreateClient();
    }

    [Fact]
    public async Task GivenValidCurrencyCodeAndWorkingApi_WhenCallingGetQuotes_ThenResponse200WithQuotes()
    {
        _factory._exchangeServiceMock.Setup(x => x.GetCurrencyQuotes(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Quote> { new Quote("EUR", 1) });
        var response = await  _client.GetAsync("/currency/TST/quotes");
        response.Should().BeSuccessful();        
    }

    [Fact]
    public async Task GivenInvalidCurrency_WhenCallingGetQuotes_ThenResponseBadRequest()
    {
        _factory._exchangeServiceMock.Setup(x => x.GetCurrencyQuotes(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Quote> { new Quote("EUR", 1) });
        var response = await _client.GetAsync("/currency/TSTS/quotes");
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.BadRequest);
    }
}
