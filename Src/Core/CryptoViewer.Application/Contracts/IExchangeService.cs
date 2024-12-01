using CryptoViewer.Domain.Entities;

namespace CryptoViewer.Application.Contracts;

public interface IExchangeService
{
    Task<List<Quote>> GetCurrencyQuotes(string  currency, CancellationToken cancellationToken);
}
