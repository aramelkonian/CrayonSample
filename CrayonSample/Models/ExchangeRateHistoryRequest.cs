using System.ComponentModel.DataAnnotations;

namespace CrayonSample.Models;

public interface IExchangeRateHistoryRequest
{
    DateTimeOffset[] Dates { get; set; }
    string FromSymbol { get; set; }
    string ToSymbol { get; set; }
}

public class ExchangeRateHistoryRequest : IExchangeRateHistoryRequest
{
    /// <summary>
    /// Collection of dates to search.
    /// </summary>
    [Required, MinLength(1), HistoricalDates]
    public DateTimeOffset[] Dates { get; set; } = Array.Empty<DateTimeOffset>();

    /// <summary>
    /// Source currency
    /// </summary>
    [Required]
    public string FromSymbol { get; set; } = string.Empty;

    /// <summary>
    /// Destination Currency
    /// </summary>
    [Required]
    public string ToSymbol { get; set; } = string.Empty;
}

public class ExchangeRateHistoryResponse
{
    /// <summary>
    /// Date and rate of the minimum rate.
    /// </summary>
    public ExchangeRateHistoryDetails? Minimum { get; set; }
    /// <summary>
    /// Date and rate of the maximum rate.
    /// </summary>
    public ExchangeRateHistoryDetails? Maximum { get; set; }
    /// <summary>
    /// The average rate.
    /// </summary>
    public decimal Average { get; set; }
}
