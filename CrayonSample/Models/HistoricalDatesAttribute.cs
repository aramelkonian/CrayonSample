using System.ComponentModel.DataAnnotations;

namespace CrayonSample.Models;

public class HistoricalDatesAttribute : ValidationAttribute
{
    private const string Error = "Please provide a date in the past.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var request = validationContext.ObjectInstance as ExchangeRateHistoryRequest;

        if (request is not null && request.Dates.Any())
        {
            if (!request.Dates.Where(x => x.Date <= DateTimeOffset.UtcNow.Date).Any())
            {
                return new ValidationResult(Error);
            }
        }
        return ValidationResult.Success;
    }
}
