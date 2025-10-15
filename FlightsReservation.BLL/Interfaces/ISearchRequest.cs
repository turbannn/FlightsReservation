
namespace FlightsReservation.BLL.Interfaces;

public interface ISearchRequest
{
    string DepartureCity { get; set; }
    string ArrivalCity { get; set; }
    DateTime DepartureDate { get; set; }
}