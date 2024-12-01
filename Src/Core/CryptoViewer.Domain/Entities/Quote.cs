using System.Text.Json.Serialization;

namespace CryptoViewer.Domain.Entities;

public record Quote {
    public string CurrencyCode { get; } 
    public decimal Rate { get; }

    [JsonConstructor]
    public Quote(string currencyCode, decimal rate) { 
        CurrencyCode = currencyCode;
        Rate = rate;
    }
}