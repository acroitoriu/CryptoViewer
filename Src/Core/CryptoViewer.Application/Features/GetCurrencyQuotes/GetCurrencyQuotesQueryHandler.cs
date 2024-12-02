using CryptoViewer.Application.Contracts;
using FluentValidation;
using MediatR;

namespace CryptoViewer.Application.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesQueryHandler : IRequestHandler<GetCurrencyQuotesQuery, GetCurrencyQuotesResponse>
{
    private readonly IValidator<GetCurrencyQuotesQuery> _validator;
    private readonly IExchangeService _exchangeService;    

    public GetCurrencyQuotesQueryHandler(IValidator<GetCurrencyQuotesQuery> validator, IExchangeService exchangeService)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _exchangeService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
    }

    public async Task<GetCurrencyQuotesResponse> Handle(GetCurrencyQuotesQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var quotes = await _exchangeService.GetCurrencyQuotes(request.CurrencyCode, cancellationToken);

        return new GetCurrencyQuotesResponse {  Quotes = quotes };
    }
}
