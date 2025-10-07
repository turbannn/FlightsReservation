using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.DAL.Interfaces;

public interface ISeatsRepository : IRepository<Seat>
{
    Task MarkSeatAsAvailable(Guid seatId);
    Task MarkSeatAsOccupied(Guid seatId);
}