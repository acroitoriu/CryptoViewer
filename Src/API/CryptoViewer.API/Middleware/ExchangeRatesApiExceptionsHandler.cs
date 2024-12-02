using CryptoViewer.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CryptoViewer.API.Middleware;

public class ExchangeRatesApiExceptionsHandler : IExceptionHandler
{
    private readonly ILogger<ExchangeRatesApiExceptionsHandler> _logger;

    public ExchangeRatesApiExceptionsHandler(ILogger<ExchangeRatesApiExceptionsHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ExchangeRatesApiException eraEx)
        {
            return false;
        }
        _logger.LogError(eraEx, "Exception occurred: {Message}", eraEx.Message);
        var problemDetails = new ProblemDetails
        {
            Status = (int)eraEx.StatusCode,
            Title = eraEx.StatusCode.ToString(),
            Type = exception.GetType().Name,
            Detail = eraEx.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
