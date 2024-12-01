using FluentValidation;

namespace CryptoViewer.Application.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesQueryValidator : AbstractValidator<GetCurrencyQuotesQuery>
{    
    public GetCurrencyQuotesQueryValidator()
    {
        RuleFor(x => x.CurrencyCode)
            .NotNull()
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Matches("^[A-Z]{3}$")
            .WithMessage("{PropertyName} is not in the current format (eg: USD, BTC, EUR...)");
    }
}
