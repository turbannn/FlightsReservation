namespace FlightsReservation.BLL.Entities.Utilities.Other;

public static class AirportCodeMapper
{
    private static readonly Dictionary<string, string> AirportCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        // Poland
        { "WAW", "Warsaw" },
        { "KRK", "Krakow" },
        { "GDN", "Gdansk" },
        { "WRO", "Wroclaw" },
        { "POZ", "Poznan" },
        { "KTW", "Katowice" },
        { "RZE", "Rzeszow" },
        { "SZZ", "Szczecin" },
        { "IEG", "Zielona Gora" },
        { "LUZ", "Lublin" },
        
        // Western Europe
        { "LHR", "London" },
        { "CDG", "Paris" },
        { "FRA", "Frankfurt" },
        { "AMS", "Amsterdam" },
        { "MUC", "Munich" },
        { "ZRH", "Zurich" },
        { "VIE", "Vienna" },
        { "BER", "Berlin" },
        { "DUS", "Dusseldorf" },
        { "CPH", "Copenhagen" },
        { "BCN", "Barcelona" },
        { "MAD", "Madrid" },
        { "ALC", "Alicante" },
        { "AGP", "Malaga" },
        { "PMI", "Palma de Mallorca" },
        
        // Northern Europe
        { "OSL", "Oslo" },
        { "STO", "Stockholm" },
        { "HEL", "Helsinki" },
        { "RIX", "Riga" },
        { "TLL", "Tallinn" },
        { "VNO", "Vilnius" },
        { "KEF", "Reykjavik" },
        
        // Southern Europe
        { "FCO", "Rome" },
        { "MXP", "Milan" },
        { "VCE", "Venice" },
        { "ATH", "Athens" },
        { "IST", "Istanbul" },
        { "LIS", "Lisbon" },
        { "DBV", "Dubrovnik" },
        { "SOF", "Sofia" },
        
        // Eastern Europe
        { "BUD", "Budapest" },
        { "PRG", "Prague" },
        { "OTP", "Bucharest" },
        { "KIV", "Chisinau" },
        
        // Middle East & Asia
        { "DXB", "Dubai" },
        { "DOH", "Doha" },
        { "DEL", "Delhi" },
        { "BOM", "Mumbai" },
        { "HKG", "Hong Kong" },
        { "SIN", "Singapore" },
        { "BKK", "Bangkok" },
        { "NRT", "Tokyo" },
        { "ICN", "Seoul" },
        { "PEK", "Beijing" },
        
        // Mediterranean & Beach Destinations
        { "AYT", "Antalya" },
        { "HRG", "Hurghada" },
        { "SSH", "Sharm el-Sheikh" },
        { "HER", "Heraklion" },
        { "RHO", "Rhodes" },
        { "CFU", "Corfu" },
        
        // Americas
        { "JFK", "New York" },
        { "LAX", "Los Angeles" },
        { "ORD", "Chicago" },
        { "MIA", "Miami" },
        { "YYZ", "Toronto" },
        { "YUL", "Montreal" },
        { "MEX", "Mexico City" },
        { "GRU", "Sao Paulo" },
        { "EZE", "Buenos Aires" },
        
        // Africa
        { "CAI", "Cairo" },
        { "JNB", "Johannesburg" },
        { "CPT", "Cape Town" },
        { "NBO", "Nairobi" },
        
        // Additional
        { "NYO", "Stockholm Skavsta" },
    };

    /// <summary>
    /// Converts IATA airport code to city name
    /// </summary>
    /// <param name="iataCode">IATA airport code (e.g., "WAW", "LHR")</param>
    /// <returns>City name or the original code if not found</returns>
    public static string GetCityName(string? iataCode)
    {
        if (string.IsNullOrWhiteSpace(iataCode))
            return "Unknown";

        // Try to get the city name from dictionary
        if (AirportCodes.TryGetValue(iataCode.Trim(), out var cityName))
            return cityName;

        // If not found, return the code itself in uppercase
        return iataCode.Trim().ToUpper();
    }

    /// <summary>
    /// Checks if the airport code exists in the dictionary
    /// </summary>
    public static bool IsKnownAirport(string? iataCode)
    {
        if (string.IsNullOrWhiteSpace(iataCode))
            return false;

        return AirportCodes.ContainsKey(iataCode.Trim());
    }

    /// <summary>
    /// Gets all supported airport codes
    /// </summary>
    public static IReadOnlyCollection<string> GetAllAirportCodes()
    {
        return AirportCodes.Keys.ToList().AsReadOnly();
    }

    /// <summary>
    /// Gets all supported cities
    /// </summary>
    public static IReadOnlyCollection<string> GetAllCities()
    {
        return AirportCodes.Values.ToList().AsReadOnly();
    }
}
