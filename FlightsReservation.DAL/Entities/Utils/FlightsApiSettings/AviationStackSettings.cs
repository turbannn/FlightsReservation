namespace FlightsReservation.DAL.Entities.Utils.FlightsApiSettings;

public class AviationStackSettings
{
    public string BaseUrl { get; set; } = null!;
    public string AccessKey { get; set; } = null!;
    public double BaseCompanyMarkup { get; set; } = 150.0;
    public List<AviationStackSettingsAirport> Airports { get; set; } = new();
    public List<FlightReservationAirplane> Airplanes { get; set; } = new();
}