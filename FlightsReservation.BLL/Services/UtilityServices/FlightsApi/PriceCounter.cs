using FlightsReservation.DAL.Entities.Utils.FlightsApiSettings;

namespace FlightsReservation.BLL.Services.UtilityServices.FlightsApi;

public class PriceCounter
{
    private readonly AviationStackSettings _aviationStackSettings;
    private readonly Dictionary<string, double> _pricePerKmCache;

    public PriceCounter(AviationStackSettings aviationStackSettings)
    {
        _aviationStackSettings = aviationStackSettings;
        
        // Build dictionary from configuration for fast lookup
        _pricePerKmCache = _aviationStackSettings.Airplanes
            .ToDictionary(
                a => a.Code.ToLower(), 
                a => a.PricePerKm, 
                StringComparer.OrdinalIgnoreCase
            );
    }
    
    // Calculates flight price based on distance between airports and aircraft type
    public int CalculatePrice(string departureCode, string arrivalCode, string? aircraftModelCode)
    {
        var departureAirport = _aviationStackSettings.Airports
            .FirstOrDefault(a => a.Code.Equals(departureCode, StringComparison.OrdinalIgnoreCase));
        
        var arrivalAirport = _aviationStackSettings.Airports
            .FirstOrDefault(a => a.Code.Equals(arrivalCode, StringComparison.OrdinalIgnoreCase));
        
        if (departureAirport == null || arrivalAirport == null)
            return 500;
        
        double distanceKm = CalculateDistance(
            departureAirport.Latitude, 
            departureAirport.Longitude,
            arrivalAirport.Latitude, 
            arrivalAirport.Longitude
        );
        
        double pricePerKm = 0.55;
        
        if (!string.IsNullOrWhiteSpace(aircraftModelCode))
        {
            var modelCodeLower = aircraftModelCode.ToLower();
            if (_pricePerKmCache.TryGetValue(modelCodeLower, out var configuredPrice))
            {
                pricePerKm = configuredPrice;
            }
        }
        
        double totalPrice = (distanceKm * pricePerKm) + _aviationStackSettings.BaseCompanyMarkup;
        
        int finalPrice = (int)Math.Round(totalPrice);
        return Math.Max(finalPrice, 200); // Min is 200 PLN
    }
    
    //Calculates the distance between two points on Earth using the Haversine formula
    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusKm = 6371.0;
        
        double lat1Rad = DegreesToRadians(lat1);
        double lon1Rad = DegreesToRadians(lon1);
        double lat2Rad = DegreesToRadians(lat2);
        double lon2Rad = DegreesToRadians(lon2);
        
        // Haversine formula
        double dLat = lat2Rad - lat1Rad;
        double dLon = lon2Rad - lon1Rad;
        
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        double distance = EarthRadiusKm * c;
        
        return distance;
    }
    
    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}