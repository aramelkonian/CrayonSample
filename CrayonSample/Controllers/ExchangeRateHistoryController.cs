using CrayonSample.Models;
using CrayonSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrayonSample.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRateHistoryController : ControllerBase
{
    private readonly ILogger<ExchangeRateHistoryController> _logger;
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateHistoryController(ILogger<ExchangeRateHistoryController> logger, IExchangeRateService exchangeRateService)
    {
        _logger = logger;
        _exchangeRateService = exchangeRateService;
    }

    /// <summary>
    /// Retrieves the history for the provided date and symbols.
    /// There is no validation for symbols. https://api.exchangerate.host/symbols should implement this and check if it is a valid symbol.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>ExchangeRateHistoryResponse</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ExchangeRateHistoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _exchangeRateService.GetHistory(request);
        return Ok(response);
    }
}
