
namespace FlightsReservation.DAL.Interfaces;

public interface IPagedRepository<TEntity> : IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetPageAsync(int page, int size);
    Task<int> GetTotalCountAsync();
}