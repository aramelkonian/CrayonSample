using CrayonSample.Models;
using CrayonSample.Models.ExchangeRateDotHost;
using System.Text.Json;

namespace CrayonSample.Services;

public interface IExchangeRateService
{
    Task<ExchangeRateHistoryResponse> GetHistory(IExchangeRateHistoryRequest request);
}

public class ExchangeRateService : IExchangeRateService
{
    private readonly ILogger<IExchangeRateService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ExchangeRateService(IHttpClientFactory httpClientFactory, ILogger<IExchangeRateService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ExchangeRateHistoryResponse> GetHistory(IExchangeRateHistoryRequest request)
    {
        var history = await History(request);

        var average = history.Values.Average();
        var minKvp = history.MinBy(x => x.Value);
        var maxKvp = history.MaxBy(x => x.Value);

        return new ExchangeRateHistoryResponse
        {
            Average = average,
            Maximum = new ExchangeRateHistoryDetails { Date = maxKvp.Key, Rate = maxKvp.Value },
            Minimum = new ExchangeRateHistoryDetails { Date = minKvp.Key, Rate = minKvp.Value },
        };
    }

    private async Task<IDictionary<string, decimal>> History(IExchangeRateHistoryRequest request)
    {
        var httpResponses = new List<Task<HttpResponseMessage>>();
        var history = new Dictionary<string, decimal>();

        var client = _httpClientFactory.CreateClient("exchangerate.host");

        // Remove duplicate dates.
        var parsedDates = request.Dates.Select(d => d.ToString("yyyy-MM-dd"))
            .Distinct();

        // Batch all requests
        foreach (var date in parsedDates)
        {
            var endpoint = $"{date}?base={request.FromSymbol}&symbols={request.ToSymbol}";
            httpResponses.Add(client.GetAsync(endpoint));
        }

        // Send requests
        var responseMessages = await Task.WhenAll(httpResponses);

        // Process responses
        foreach (var httpResponse in responseMessages)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                var details = JsonSerializer.Deserialize<ExchangeRateDetails>(await httpResponse.Content.ReadAsStreamAsync());

                if (details is not null && !history.ContainsKey(details.Date))
                {
                    history.Add(details.Date, details.Rates.FirstOrDefault().Value);
                }
            }
            else
            {
                _logger.LogError($"Failed request ({httpResponse.StatusCode}) {await httpResponse.Content.ReadAsStringAsync()}", httpResponse);
            }
        }

        return history;
    }
}
