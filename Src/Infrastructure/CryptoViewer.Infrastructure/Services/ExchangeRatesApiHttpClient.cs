using CryptoViewer.Application.Contracts;
using CryptoViewer.Domain.Entities;
using CryptoViewer.Infrastructure.Configuration;
using CryptoViewer.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CryptoViewer.Infrastructure.Services;

public class ExchangeRatesApiHttpClient : IExchangeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRatesApiHttpClient> _logger;
    private readonly ExchangeServiceSettings _exchangeServiceSettings;
    public ExchangeRatesApiHttpClient(HttpClient httpClient, ILogger<ExchangeRatesApiHttpClient> logger, IOptions<ExchangeServiceSettings> exchangeServiceSettings)
    {        
        _httpClient = httpClient;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _exchangeServiceSettings = exchangeServiceSettings.Value;
    }

    public async Task<List<Quote>> GetCurrencyQuotes(string currency, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.GetAsync($"/latest?base={currency}&access_key={_exchangeServiceSettings.AccessKey}", cancellationToken);
        if(!httpResponse.IsSuccessStatusCode)
        {
            // error handling
            _logger.LogError("The call to get the latest rates failed. Status Code: {StatusCode}, Reason: {Reason}", httpResponse.StatusCode.ToString(), httpResponse.ReasonPhrase);            
            return new List<Quote>(); // see what exactly to do
        }

        var contentJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        var quotes = JsonSerializer.Deserialize<ExchangeRatesApi.LatestResponse>(contentJson);
        return quotes.Rates.Select(x => new Quote(currencyCode: x.Key, rate: x.Value)).ToList();
    }
}
