using MediatR;

namespace CryptoViewer.Application.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesQuery : IRequest<GetCurrencyQuotesResponse>
{
    public string CurrencyCode { get; init; }
}
