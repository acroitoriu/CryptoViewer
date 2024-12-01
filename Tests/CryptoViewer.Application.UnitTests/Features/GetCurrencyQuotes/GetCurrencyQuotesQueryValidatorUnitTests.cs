using CryptoViewer.Application.Features.GetCurrencyQuotes;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace CryptoViewer.Application.UnitTests.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesQueryValidatorUnitTests
{
    private GetCurrencyQuotesQueryValidator validator;
    
    public GetCurrencyQuotesQueryValidatorUnitTests()
    {
        validator = new GetCurrencyQuotesQueryValidator();
    }    

    [Theory]
    [InlineData(null, "Currency Code is required")]
    [InlineData("", "Currency Code is required")]
    [InlineData("abc", "Currency Code is not in the current format (eg: USD, BTC, EUR...)")]
    [InlineData("abcd", "Currency Code is not in the current format (eg: USD, BTC, EUR...)")]
    [InlineData("ABCD", "Currency Code is not in the current format (eg: USD, BTC, EUR...)")]
    [InlineData("AB", "Currency Code is not in the current format (eg: USD, BTC, EUR...)")]
    [InlineData("B", "Currency Code is not in the current format (eg: USD, BTC, EUR...)")]
    [InlineData("$", "Currency Code is not in the current format (eg: USD, BTC, EUR...)")]
    public void InvalidCurrencyCode_ReturnsValidationErrors(string currencyCode, string expectedError)
    {        
        var validationResult = validator.TestValidate(new GetCurrencyQuotesQuery { CurrencyCode = currencyCode });
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.ShouldHaveValidationErrorFor(x => x.CurrencyCode).WithErrorMessage(expectedError);        
    }


    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("BTC")]
    public void ValidCurrencyCode_ReturnsIsValid(string currencyCode)
    {        
        var validationResult = validator.TestValidate(new GetCurrencyQuotesQuery { CurrencyCode = currencyCode });
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();        
    }

}
