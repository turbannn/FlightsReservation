using Microsoft.EntityFrameworkCore;
using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;

namespace FlightsReservation.DAL.Repositories;

public class FlightsRepository : IFlightsRepository
{
    private readonly FlightsDbContext _context;
    public FlightsRepository(FlightsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<IEnumerable<Flight>> GetPageAsync(int page, int size, CancellationToken ct)
    {
        return await _context.Flights
            .AsNoTracking()
            .Include(f => f.Seats)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.Passengers)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken ct)
    {
        return await _context.Flights.CountAsync(cancellationToken: ct);
    }

    public async Task<Flight?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var flight = await _context.Flights
            .AsNoTracking()
            .Include(f => f.Seats)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.Passengers)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);

        return flight;
    }

    public async Task AddAsync(Flight entityToAdd, CancellationToken ct = default)
    {
        await _context.Flights.AddAsync(entityToAdd, ct);
    }

    public async Task UpdateAsync(Flight entityToUpdate, CancellationToken ct = default)
    {
        await _context.Flights
            .Where(f => f.Id == entityToUpdate.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ff => ff.FlightNumber, entityToUpdate.FlightNumber)
                .SetProperty(ff => ff.Departure, entityToUpdate.Departure)
                .SetProperty(ff => ff.Arrival, entityToUpdate.Arrival)
                .SetProperty(ff => ff.DepartureTime, entityToUpdate.DepartureTime)
                .SetProperty(ff => ff.ArrivalTime, entityToUpdate.ArrivalTime)
                .SetProperty(ff => ff.AirplaneType, entityToUpdate.AirplaneType), cancellationToken: ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id, cancellationToken: ct);
        
        if(flight is null) return;

        _context.Flights.Remove(flight);
    }
}

