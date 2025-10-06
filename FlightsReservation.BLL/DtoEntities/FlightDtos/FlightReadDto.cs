using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.DtoEntities.FlightDtos;

public class FlightReadDto : BaseTransferEntity, IFlightDto
{
    public string FlightNumber { get; set; }
    public string Departure { get; set; }
    public string Arrival { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string AirplaneType { get; set; }

    public int AvailableSeats { get; set; }
}
