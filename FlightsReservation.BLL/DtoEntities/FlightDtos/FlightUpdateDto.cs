using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.FlightDtos;

public class FlightUpdateDto : BaseTransferEntity, IFlightDto
{
    public string FlightNumber { get; set; } = null!;
    public string Departure { get; set; } = null!;
    public string Arrival { get; set; } = null!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string AirplaneType { get; set; } = null!;
}
