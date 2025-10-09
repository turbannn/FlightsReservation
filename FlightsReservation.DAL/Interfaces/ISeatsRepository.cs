using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.DAL.Interfaces;

public interface ISeatsRepository : IRepository<Seat>
{
    Task MarkSeatAsAvailable(Guid seatId, CancellationToken ct);
    Task MarkSeatAsOccupied(Guid seatId, CancellationToken ct);
}