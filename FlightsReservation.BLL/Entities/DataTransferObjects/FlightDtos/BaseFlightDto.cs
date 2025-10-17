using System.Text.Json.Serialization;
using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

public class BaseFlightDto : IFlightDto
{
    [JsonIgnore]
    public string FlightNumber { get; set; } = null!;
    public string Departure { get; set; } = null!;
    public string Arrival { get; set; } = null!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string AirplaneType { get; set; } = null!;
}
