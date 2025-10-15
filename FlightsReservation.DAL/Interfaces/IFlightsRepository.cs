using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Entities.Utils;

namespace FlightsReservation.DAL.Interfaces;

public interface IFlightsRepository : IPagedRepository<Flight>
{
    Task<IEnumerable<Flight>> GetFilteredPageAsync(int page, 
        int size,
        string departure,
        string arrival,
        DateTime departureTime,
        CancellationToken ct);

    Task<FlightsWithReturnResult> GetFilteredPageWithReturnAsync(int page,
        int size,
        string departure,
        string arrival,
        DateTime departureTime,
        DateTime returnTime,
        CancellationToken ct);
}