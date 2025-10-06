using Microsoft.EntityFrameworkCore.Storage;
using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Interfaces;

namespace FlightsReservation.DAL.UoWs;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly FlightsDbContext _db;
    private IDbContextTransaction? _tx;
    private bool _disposed;

    public EfUnitOfWork(FlightsDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task BeginAsync(CancellationToken ct = default)
    {
        if (_tx is not null) return;
        _tx = await _db.Database.BeginTransactionAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _db.SaveChangesAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_tx is null) return;
        await _db.SaveChangesAsync(ct);
        await _tx.CommitAsync(ct);
        await _tx.DisposeAsync();
        _tx = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_tx is null) return;
        await _tx.RollbackAsync(ct);
        await _tx.DisposeAsync();
        _tx = null;
    }
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        if (_tx is not null)
        {
            try { await _tx.RollbackAsync(); } catch { /* swallow */ }
            await _tx.DisposeAsync();
            _tx = null;
        }

        _disposed = true;
    }
}

