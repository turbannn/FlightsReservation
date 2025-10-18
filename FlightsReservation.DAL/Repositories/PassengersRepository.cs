using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightsReservation.DAL.Repositories;

public class PassengersRepository : IPassengersRepository
{
    private readonly FlightsDbContext _context;

    public PassengersRepository(FlightsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Passenger?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _context.Passengers
            .AsNoTracking()
            .Include(p => p.Seat)
            .Include(p => p.Reservation)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);

        return p;
    }

    public async Task<bool> AddAsync(Passenger entityToAdd, CancellationToken ct = default)
    {
        try
        {
            await _context.Passengers.AddAsync(entityToAdd, ct);
            return true;
        }
        catch
        {
            return false;
        }

    }

    public bool Update(Passenger entityToUpdate)
    {
        _context.Passengers.Update(entityToUpdate);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _context.Passengers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);

        if (p is null)
        {
            return false;
        }

        _context.Passengers.Remove(p);
        return true;
    }
}

