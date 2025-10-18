using FlightsReservation.BLL.Interfaces.Requests;

namespace FlightsReservation.BLL.Entities.Utilities.Requests;

public class BaseSearchRequest : ISearchRequest
{
    public string DepartureCity { get; set; } = null!;
    public string ArrivalCity { get; set; } = null!;
    public DateTime DepartureDate { get; set; }
}