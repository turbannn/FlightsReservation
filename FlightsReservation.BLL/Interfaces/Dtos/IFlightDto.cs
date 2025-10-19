namespace FlightsReservation.BLL.Interfaces.Dtos;

public interface IFlightDto
{
    string Departure { get; set; }
    string Arrival { get; set; }
    DateTime DepartureTime { get; set; }
    DateTime ArrivalTime { get; set; }
    string AirplaneType { get; set; }
    int Price { get; set; }
    string Currency { get; set; }
    string Company { get; set; }
}