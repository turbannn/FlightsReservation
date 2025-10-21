using FlightsReservation.BLL.Interfaces.Requests;

namespace FlightsReservation.BLL.Entities.Utilities.Requests;

public class BaseSearchRequest : ISearchRequest
{
    private DateTime _departureDate;
    public string DepartureCity { get; set; } = null!;
    public string ArrivalCity { get; set; } = null!;
    public DateTime DepartureDate
    {
        get => _departureDate;
        set => _departureDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}