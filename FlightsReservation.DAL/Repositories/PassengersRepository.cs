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

    public async Task<bool> UpdateAsync(Passenger entityToUpdate, CancellationToken ct = default)
    {
        var res = await _context.Passengers
            .Where(p => p.Id == entityToUpdate.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(pp => pp.FirstName, entityToUpdate.FirstName)
                .SetProperty(pp => pp.LastName, entityToUpdate.LastName)
                .SetProperty(pp => pp.PassportNumber, entityToUpdate.PassportNumber)
                .SetProperty(pp => pp.PhoneNumber, entityToUpdate.PhoneNumber)
                .SetProperty(pp => pp.Email, entityToUpdate.Email), cancellationToken: ct);

        if (res <= 0)
            return false;

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

