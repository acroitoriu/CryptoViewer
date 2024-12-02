using CryptoViewer.Application.Features.GetCurrencyQuotes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CryptoViewer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {        
        private readonly ILogger<CurrencyController> _logger;
        private readonly IMediator _mediatr;

        public CurrencyController(IMediator mediatr,  ILogger<CurrencyController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr)); ;
        }

        // the route constrain is at the moment alpha, BUT could be  regex expression to limit to 3 character input
        // left it as alpha so I can hit the validation in the application layer
        [HttpGet("{currencyCode:alpha}/quotes", Name = "GetCurrencyQuotes")]
        [ProducesResponseType(typeof(GetCurrencyQuotesResponse), 200)]
        public async Task<IActionResult> Get(string currencyCode)
        {
            _logger.LogInformation("Trying to get the quotes for CurrencyCode: {CurrencyCode}", currencyCode);
            var queryResponse = await _mediatr.Send(new GetCurrencyQuotesQuery { CurrencyCode = currencyCode });
            return Ok(queryResponse);
        }
    }
}
