
using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightsReservation.DAL.Repositories;

public class ReservationRepository : IReservationsRepository
{
    private readonly FlightsDbContext _context;
    public ReservationRepository(FlightsDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var r = await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Flight)
            .Include(r => r.Passengers)
            .ThenInclude(p => p.Seat)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken: ct);

        return r;
    }

    public async Task<bool> AddAsync(Reservation entityToAdd, CancellationToken ct = default)
    {
        try
        {
            await _context.Reservations.AddAsync(entityToAdd, ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Reservation entityToUpdate, CancellationToken ct = default)
    {
        var res = await _context.Reservations
            .Where(r => r.Id == entityToUpdate.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(rr => rr.ReservationNumber, entityToUpdate.ReservationNumber)
                .SetProperty(rr => rr.ReservationDate, entityToUpdate.ReservationDate), cancellationToken: ct);

        if (res <= 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var res = await _context.Reservations.Where(r => r.Id == id).ExecuteDeleteAsync(cancellationToken: ct);

        if(res <= 0)
            return false;

        return true;
    }
}