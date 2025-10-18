
namespace FlightsReservation.DAL.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> AddAsync(TEntity entityToAdd, CancellationToken ct);
    bool Update(TEntity entityToUpdate);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}