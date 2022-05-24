# CrayonSample

## Terminal
Requires **.NET 6** to run the application.  
Open a terminal at the root of the application and enter `dotnet run`.  

``` bash
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7256
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5256
```
You can access the OpenApi documentation at `https://localhost:7256/swagger/index.html`

## Visual Studio 2022
Open the `CrayonSample.sln`.  
Press `F5` or click `Run`.  

> You may be prompted to install the dev certificate.  

## Request
```
https://localhost:7256/ExchangeRateHistory?Dates=2022-05-24&Dates=2022-05-23&Dates=2022-05-22&FromSymbol=USD&ToSymbol=AUD
```

## Response
``` json
{
    "minimum": {
        "rate": 1.41069,
        "date": "2022-05-23"
    },
    "maximum": {
        "rate": 1.414371,
        "date": "2022-05-22"
    },
    "average": 1.412689
}
```

# Limitations
* There is no validation for symbols. https://api.exchangerate.host/symbols should implement this and check if it is a valid symbol.  
* Only successful requests to https://api.exchangerate.host/ are processed. Could potentially include a list of failed requests (dates).  
* Duplicate dates are silently discarded. 
* Basic request validation.  
> Required fields, array length and check for past dates - an invalid date could still be provided if it is too far in the past.
