
namespace FlightsReservation.BLL.Entities.Utilities.Requests;

public class FlightSearchWithReturnRequest : BaseSearchRequest
{
    public DateTime ReturnDate { get; set; }
}