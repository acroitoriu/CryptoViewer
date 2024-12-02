using CryptoViewer.Application.Contracts;
using CryptoViewer.Domain.Entities;
using CryptoViewer.Infrastructure.Configuration;
using CryptoViewer.Infrastructure.Exceptions;
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
        var httpResponse = await _httpClient.GetAsync($"/latest?base={currency}&symbols={string.Join(',', _exchangeServiceSettings.QuotesToFetch)}&access_key={_exchangeServiceSettings.AccessKey}", cancellationToken);
        if(!httpResponse.IsSuccessStatusCode)
        {
            var errorContentJson = await httpResponse.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ExchangeRatesApi.ErrorResponse>(errorContentJson);                        
            _logger.LogError("The HTTP call to get the latest rates failed. Status Code: {StatusCode}, Reason: {Reason}", httpResponse.StatusCode.ToString(), httpResponse.ReasonPhrase);            
            throw new ExchangeRatesApiException(httpResponse.StatusCode, errorContentJson);
        }

        var contentJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        var quotes = JsonSerializer.Deserialize<ExchangeRatesApi.LatestResponse>(contentJson);
        if (!quotes.Success)
        {
            _logger.LogError("The HTTP call returned 200 OK, but Success flag is false.");
            throw new ExchangeRatesApiException(System.Net.HttpStatusCode.BadRequest, JsonSerializer.Serialize(quotes.Error));
        }

        return quotes.Rates                
                .Select(x => new Quote(currencyCode: x.Key, rate: x.Value))
                .ToList();

    }
}
