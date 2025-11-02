namespace FlightsReservation.DAL.Entities.Utils.FlightsApiSettings;

public class AviationStackSettings
{
    public string BaseUrl { get; set; } = null!;
    public string AccessKey { get; set; } = null!;
    public List<AviationStackSettingsAirport> Airports { get; set; } = new();
}