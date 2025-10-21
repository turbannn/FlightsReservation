
namespace FlightsReservation.BLL.Entities.Utilities.Requests;

public class FlightSearchWithReturnRequest : BaseSearchRequest
{
    private DateTime _departureDate;
    public DateTime ReturnDate
    {
        get => _departureDate;
        set => _departureDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}