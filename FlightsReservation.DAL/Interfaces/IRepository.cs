
namespace FlightsReservation.DAL.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(TEntity entityToAdd, CancellationToken ct);
    Task UpdateAsync(TEntity entityToUpdate, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
