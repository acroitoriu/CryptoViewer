using CryptoViewer.Domain.Entities;

namespace CryptoViewer.Application.Features.GetCurrencyQuotes;

public class GetCurrencyQuotesResponse
{
    public List<Quote> Quotes { get; set; } = new List<Quote>();
}