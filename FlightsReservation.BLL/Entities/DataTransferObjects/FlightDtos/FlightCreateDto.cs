using System.Text.Json.Serialization;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

public class FlightCreateDto : BaseFlightDto
{
    [JsonIgnore]
    public string FlightNumber { get; set; } = null!;
}