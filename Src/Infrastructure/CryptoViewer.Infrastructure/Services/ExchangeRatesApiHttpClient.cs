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
    public ExchangeRatesApiHttpClient(IHttpClientFactory httpClientFactory, ILogger<ExchangeRatesApiHttpClient> logger, IOptions<ExchangeServiceSettings> exchangeServiceSettings)
    {
        _httpClient = httpClientFactory.CreateClient("exchangeRatesApi");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _exchangeServiceSettings = exchangeServiceSettings.Value;
    }

    public async Task<List<Quote>> GetCurrencyQuotes(string currency, CancellationToken cancellationToken)
    {
        var httpResponse = _httpClient.GetAsync($"/latest?base={currency}&access_key={_exchangeServiceSettings.AccessKey}", cancellationToken);
        if(!httpResponse.IsCompletedSuccessfully || !httpResponse.Result.IsSuccessStatusCode)
        {
            // error handling
            _logger.LogError("The call to get the latest rates failed. Status Code: {StatusCode}, Reason: {Reason}", httpResponse.Result.StatusCode.ToString(), httpResponse.Result.ReasonPhrase);            
            return new List<Quote>(); // see what exactly to do
        }

        var contentJson = await httpResponse.Result.Content.ReadAsStringAsync(cancellationToken);
        var quotes = JsonSerializer.Deserialize<ExchangeRatesApi.LatestResponse>(contentJson);
        return quotes.Rates.Select(x => new Quote(currencyCode: x.Key, rate: x.Value)).ToList();
    }
}
