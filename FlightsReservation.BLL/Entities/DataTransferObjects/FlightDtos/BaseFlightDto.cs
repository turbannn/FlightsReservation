using FlightsReservation.BLL.Interfaces.Dtos;

namespace FlightsReservation.BLL.Entities.DataTransferObjects.FlightDtos;

public class BaseFlightDto : IFlightDto
{
    public string Departure { get; set; } = null!;
    public string Arrival { get; set; } = null!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string AirplaneType { get; set; } = null!;
    public double Price { get; set; }
    public string Currency { get; set; } = null!;
    public string Company { get; set; } = null!;
}
