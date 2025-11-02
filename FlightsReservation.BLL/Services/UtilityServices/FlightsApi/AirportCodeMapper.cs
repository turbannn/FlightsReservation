using FlightsReservation.DAL.Entities.Utils.FlightsApiSettings;

namespace FlightsReservation.BLL.Services.UtilityServices.FlightsApi;

public class AirportCodeMapper
{
    private readonly Dictionary<string, AviationStackSettingsAirport> _airportCodes;

    public AirportCodeMapper(AviationStackSettings settings)
    {
        _airportCodes = settings.Airports
            .ToDictionary(a => a.Code, a => a, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Converts IATA airport code to city name
    /// </summary>
    /// <param name="iataCode">IATA airport code (e.g., "WAW", "LHR")</param>
    /// <returns>City name or the original code if not found</returns>
    public string GetCityName(string? iataCode)
    {
        if (string.IsNullOrWhiteSpace(iataCode))
            return "Unknown";

        // Try to get the city name from dictionary
        if (_airportCodes.TryGetValue(iataCode.Trim(), out var airport))
            return airport.City;

        // If not found, return the code itself in uppercase
        return iataCode.Trim().ToUpper();
    }

    /// <summary>
    /// Checks if the airport code exists in the dictionary
    /// </summary>
    public bool IsKnownAirport(string? iataCode)
    {
        if (string.IsNullOrWhiteSpace(iataCode))
            return false;

        return _airportCodes.ContainsKey(iataCode.Trim());
    }

    /// <summary>
    /// Gets all supported airport codes
    /// </summary>
    public IReadOnlyCollection<string> GetAllAirportCodes()
    {
        return _airportCodes.Keys.ToList().AsReadOnly();
    }

    /// <summary>
    /// Gets all supported cities
    /// </summary>
    public IReadOnlyCollection<string> GetAllCities()
    {
        return _airportCodes.Values.Select(a => a.City).ToList().AsReadOnly();
    }

    /// <summary>
    /// Gets airport information by code
    /// </summary>
    public AviationStackSettingsAirport? GetAirport(string? iataCode)
    {
        if (string.IsNullOrWhiteSpace(iataCode))
            return null;

        _airportCodes.TryGetValue(iataCode.Trim(), out var airport);
        return airport;
    }
}
