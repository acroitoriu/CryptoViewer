using FluentValidation;
using MediatR;

namespace CryptoViewer.Application.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesQueryHandler : IRequestHandler<GetCurrencyQuotesQuery, GetCurrencyQuotesResponse>
{
    private readonly IValidator<GetCurrencyQuotesQuery> _validator;

    public GetCurrencyQuotesQueryHandler(IValidator<GetCurrencyQuotesQuery> validator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<GetCurrencyQuotesResponse> Handle(GetCurrencyQuotesQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return await Task.FromResult(new GetCurrencyQuotesResponse());
    }
}
