
namespace FlightsReservation.DAL.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entityToAdd);
    Task UpdateAsync(TEntity entityToUpdate);
    Task DeleteAsync(Guid id);
}
