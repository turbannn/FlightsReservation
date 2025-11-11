using FlightsReservation.DAL.Entities.Model;

namespace FlightsReservation.DAL.Interfaces;

public interface IUsersRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken ct);
    Task<bool> UpdateMoneyAsync(Guid id, int amount, CancellationToken ct);
    Task<User?> GetByUsernameAsync(string login, CancellationToken ct);
    Task<bool> AddMoneyAsync(Guid id, double amount, CancellationToken ct);
    Task<bool> SubtractMoneyAsync(Guid id, double amount, CancellationToken ct);
}