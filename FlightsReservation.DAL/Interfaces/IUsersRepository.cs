using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.DAL.Interfaces;

public interface IUsersRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken ct);
}