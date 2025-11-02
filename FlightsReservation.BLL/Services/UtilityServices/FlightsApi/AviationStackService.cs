using System.Net;
using System.Text.Json;
using FlightsReservation.BLL.Entities.Utilities.Other;
using FlightsReservation.BLL.Entities.Utilities.Results;
using FlightsReservation.DAL.Entities.Utils.FlightsApiSettings;

namespace FlightsReservation.BLL.Services.UtilityServices.FlightsApi;

public class AviationStackService
{
    private readonly HttpClient _httpClient;
    private readonly AviationStackSettings _settings;

    public AviationStackService(AviationStackSettings settings, IHttpClientFactory clientFactory)
    {
        _settings = settings;
        _httpClient = clientFactory.CreateClient("PayU");
    }

    public async Task<FlightReservationResult<List<AviationStackFlight>>> GetFutureFlightsAsync(string iataCode, string type, DateTime date)
    {
        var (isSuccess, content) = await GetFutureFlightsFromApiAsync(iataCode, type, date);

        if (!isSuccess)
        {
            Console.WriteLine("Falling back to provided JSON content.");
            return FlightReservationResult<List<AviationStackFlight>>.Fail(content, ResponseCodes.BadRequest); 
        }

        try
        {
            Console.WriteLine(content.Substring(400));
            var response = JsonSerializer.Deserialize<AviationStackResponse>(content);

            if (response?.Data == null || response.Data.Count == 0)
            {
                Console.WriteLine("No flights found in the JSON content.");
                return FlightReservationResult<List<AviationStackFlight>>.Fail("No flights found in the JSON content.", ResponseCodes.NotFound);
            }

            Console.WriteLine($"Successfully parsed {response.Data.Count} flights from JSON.");
            return FlightReservationResult<List<AviationStackFlight>>.Success(response.Data, ResponseCodes.Success);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON parsing failed: {ex.Message}");
            return FlightReservationResult<List<AviationStackFlight>>.Fail($"JSON parsing failed: {ex.Message}", ResponseCodes.InternalServerError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return FlightReservationResult<List<AviationStackFlight>>.Fail($"Unexpected error: {ex.Message}", ResponseCodes.InternalServerError);
        }
    }

    private async Task<(bool IsSuccess, string Content)> GetFutureFlightsFromApiAsync(string iataCode, string type, DateTime date)
    {
        var dateStr = date.ToString("yyyy-MM-dd");

        // flightsFuture requires a date greater than 7 days from now
        var minAllowed = DateTime.UtcNow.Date.AddDays(7);
        if (date.Date <= minAllowed)
        {
            Console.WriteLine($"FlightsFuture requires a date greater than {minAllowed:yyyy-MM-dd}. Requested: {dateStr}");
            return new ValueTuple<bool, string>(false, $"FlightsFuture requires a date greater than {minAllowed:yyyy-MM-dd}. Requested: {dateStr}");
        }

        // build query safely and URL-encode values
        var query = new Dictionary<string, string>
        {
            ["access_key"] = _settings.AccessKey,
            ["iataCode"] = iataCode,
            ["type"] = type,
            ["date"] = dateStr
        };

        var url = "flightsFuture?" + string.Join("&", query.Select(kv => WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value)));

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine("HTTP request failed: " + ex.Message);
            return new ValueTuple<bool, string>(false, "HTTP request failed: " + ex.Message);
        }

        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"API returned {(int)response.StatusCode} {response.ReasonPhrase}:");
            Console.WriteLine(content);
            return new ValueTuple<bool, string>(false, $"API returned {(int)response.StatusCode} {response.ReasonPhrase}:");
        }

        return new ValueTuple<bool, string>(true, content);
    }
}