using System.ComponentModel.DataAnnotations;

namespace CryptoViewer.Infrastructure.Configuration;

public class ExchangeServiceSettings
{
    [Required]
    public string BaseUrl { get; set; }
    [Required]
    public string AccessKey { get; set; }
}
