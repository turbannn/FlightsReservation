
namespace FlightsReservation.DAL.Interfaces;

public interface IPagedRepository<TEntity> : IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetPageAsync(int page, int size, CancellationToken ct);
    Task<int> GetTotalCountAsync(CancellationToken ct);
}