namespace FlightsReservation.BLL.Interfaces.Requests;

public interface ISearchRequest
{
    string DepartureCity { get; set; }
    string ArrivalCity { get; set; }
    DateTime DepartureDate { get; set; }
}