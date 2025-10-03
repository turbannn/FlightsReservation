using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.DAL.Interfaces;

public interface ISeatsRepository : IRepository<Seat>
{
    Task MarkSeatAsAvailable(int seatId);
    Task MarkSeatAsOccupied(int seatId);
}