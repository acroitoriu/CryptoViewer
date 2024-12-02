using System.ComponentModel.DataAnnotations;

namespace CryptoViewer.Infrastructure.Configuration;

public class ExchangeServiceSettings
{
    [Required, Url]
    public string BaseUrl { get; set; }
    [Required]
    public string AccessKey { get; set; }
    [Required]
    public List<string> QuotesToFetch { get; set; } // list of currency code for which the quotes need to be fetched
}
