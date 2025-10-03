
namespace FlightsReservation.DAL.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(int id);
    Task AddAsync(TEntity entityToAdd);
    Task UpdateAsync(TEntity entityToUpdate);
    Task DeleteAsync(int id);
}
