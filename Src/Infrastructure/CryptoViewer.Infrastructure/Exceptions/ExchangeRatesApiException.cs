using CryptoViewer.Application.Exceptions;
using System.Net;

namespace CryptoViewer.Infrastructure.Exceptions;

public class ExchangeRatesApiException : ExchangeServiceException
{
    public HttpStatusCode StatusCode { get; init; }
    public ExchangeRatesApiException(HttpStatusCode statusCode) {
        StatusCode = statusCode;
    }
    public ExchangeRatesApiException(HttpStatusCode statusCode, string message) : base(message) {
        StatusCode = statusCode;
    }

    public ExchangeRatesApiException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner) {
        StatusCode = statusCode;
    }
}
