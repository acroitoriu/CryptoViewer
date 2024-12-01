using CryptoViewer.Application.Features.GetCurrencyQuotes;
using FluentAssertions;
using FluentValidation;


namespace CryptoViewer.Application.UnitTests.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesQueryHandlerUnitTests
{
    private GetCurrencyQuotesQueryValidator validator;
    private GetCurrencyQuotesQueryHandler handler;

    public GetCurrencyQuotesQueryHandlerUnitTests()
    {
        validator = new GetCurrencyQuotesQueryValidator();
        handler = new GetCurrencyQuotesQueryHandler(validator);
    }

    [Theory]
    [InlineData("")]
    [InlineData("acd")]
    [InlineData("ABCD")]
    public async Task InvalidCurrencyCode_QueryHandler_ThrowsValidationException(string currencyCode)
    {
        var query = new GetCurrencyQuotesQuery { CurrencyCode = currencyCode };
        await handler.Awaiting(x => x.Handle(query, CancellationToken.None)).Should().ThrowAsync<ValidationException>();                
    }
}
