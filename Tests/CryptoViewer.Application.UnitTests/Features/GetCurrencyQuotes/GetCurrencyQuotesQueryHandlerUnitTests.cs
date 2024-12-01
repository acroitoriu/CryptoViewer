using CryptoViewer.Application.Contracts;
using CryptoViewer.Application.Features.GetCurrencyQuotes;
using CryptoViewer.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Moq;


namespace CryptoViewer.Application.UnitTests.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesQueryHandlerUnitTests
{
    private GetCurrencyQuotesQueryValidator _validator;
    private GetCurrencyQuotesQueryHandler _handler;
    private Mock<IExchangeService> _exchangeServiceMock;

    public GetCurrencyQuotesQueryHandlerUnitTests()
    {
        _validator = new GetCurrencyQuotesQueryValidator();
        _exchangeServiceMock = new Mock<IExchangeService>();
        _handler = new GetCurrencyQuotesQueryHandler(_validator, _exchangeServiceMock.Object);

    }

    [Theory]
    [InlineData("")]
    [InlineData("acd")]
    [InlineData("ABCD")]
    public async Task InvalidCurrencyCode_QueryHandler_ThrowsValidationException(string currencyCode)
    {
        var query = new GetCurrencyQuotesQuery { CurrencyCode = currencyCode };
        await _handler.Awaiting(x => x.Handle(query, CancellationToken.None)).Should().ThrowAsync<ValidationException>();                
    }

    [Fact]
    public async Task ValidCurrencyCodeAndSuccessfulGetQuotesCall_QueryHandler_ReturnsQueryResponse()
    {
        var query = new GetCurrencyQuotesQuery { CurrencyCode = "BTC" };
        var quotes = new List<Quote>
        {
            new Quote(currencyCode: "USD", rate: 1.2m),
            new Quote(currencyCode: "EUR", rate: 1.1m),
            new Quote(currencyCode: "BRL", rate: 2m),
            new Quote(currencyCode: "GBP", rate: 1.5m),
            new Quote(currencyCode: "AUD", rate: 1.7m),
        };
        _exchangeServiceMock.Setup(x => x.GetCurrencyQuotes(It.Is<string>(x => x.Equals(query.CurrencyCode)), It.IsAny<CancellationToken>())).ReturnsAsync(quotes);
        var queryResponse = await _handler.Handle(query, CancellationToken.None);
        queryResponse.Should().NotBeNull();
        queryResponse.Quotes.Should().HaveCount(quotes.Count);
        queryResponse.Quotes.Should().Equal(quotes);
    }
}
