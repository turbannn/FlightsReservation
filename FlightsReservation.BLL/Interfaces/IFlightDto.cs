namespace FlightsReservation.BLL.Interfaces;

public interface IFlightDto
{
    string FlightNumber { get; set; }
    string Departure { get; set; }
    string Arrival { get; set; }
    DateTime DepartureTime { get; set; }
    DateTime ArrivalTime { get; set; }
    string AirplaneType { get; set; }
}