namespace FlightsReservation.DAL.Entities.Utils.FlightsApiSettings;

public class AviationStackSettingsAirport
{
    public string Code { get; set; } = null!;
    public string City { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
