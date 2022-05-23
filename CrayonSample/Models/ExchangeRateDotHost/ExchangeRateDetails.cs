using System.Text.Json.Serialization;

namespace CrayonSample.Models.ExchangeRateDotHost
{
    public class ExchangeRateDetails
    {
        [JsonPropertyName("base")]
        public string Base { get; set; } = string.Empty;
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();
    }
}
