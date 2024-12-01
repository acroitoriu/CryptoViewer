using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoViewer.Infrastructure.Models;

public class ExchangeRatesApi
{
    public class LatestResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Timestamp { get; set; }
        [JsonPropertyName("base")]
        public string Base { get; set; }
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("code")]
        public string Code {  get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}

public class UnixDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.UnixEpoch.AddSeconds(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue((value - DateTime.UnixEpoch).TotalMilliseconds + "000");
    }
}
