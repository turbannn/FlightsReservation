using Microsoft.EntityFrameworkCore;
using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using FlightsReservation.DAL.Entities.Utils.Result;

namespace FlightsReservation.DAL.Repositories;


public class FlightsRepository : IFlightsRepository
{
    private readonly FlightsDbContext _context;
    public FlightsRepository(FlightsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<IEnumerable<Flight>> GetFilteredPageAsync(int page,
        int size,
        string departure,
        string arrival,
        DateTime departureTime,
        CancellationToken ct)
    {
        var depLower = departure.ToLower();
        var arrLower = arrival.ToLower();

        return await _context.Flights
            .AsNoTracking()
            .AsSplitQuery()
            .Where(f =>
                f.Departure.ToLower() == depLower &&
                f.Arrival.ToLower() == arrLower &&
                f.DepartureTime >= departureTime.Date &&
                f.DepartureTime < departureTime.Date.AddDays(1))
            .Include(f => f.Seats.Where(s => s.IsAvailable && DateTime.UtcNow.AddHours(2) > s.Lock))
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<FlightsWithReturnResult> GetFilteredPageWithReturnAsync(int page,
        int size,
        string departure,
        string arrival,
        DateTime departureTime,
        DateTime returnTime,
        CancellationToken ct)
    {
        var depLower = departure.ToLower();
        var arrLower = arrival.ToLower();

        var flights = await _context.Flights
            .AsNoTracking()
            .AsSplitQuery()
            .Where(f =>
                f.Departure.ToLower() == depLower &&
                f.Arrival.ToLower() == arrLower &&
                f.DepartureTime >= departureTime.Date &&
                f.DepartureTime < departureTime.Date.AddDays(1))
            .Include(f => f.Seats.Where(s => s.IsAvailable && DateTime.UtcNow.AddHours(2) > s.Lock))
            .OrderBy(f => f.DepartureTime)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken: ct);

        var flightsBack = await _context.Flights
            .AsNoTracking()
            .AsSplitQuery()
            .Where(f =>
                f.Departure.ToLower() == arrLower &&
                f.Arrival.ToLower() == depLower &&
                f.DepartureTime >= returnTime.Date && f.DepartureTime < returnTime.Date.AddDays(1))
            .Include(f => f.Seats.Where(s => s.IsAvailable && DateTime.UtcNow.AddHours(2) > s.Lock))
            .OrderBy(f => f.DepartureTime)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken: ct);


        return new FlightsWithReturnResult(flights, flightsBack);
    }

    public async Task<IEnumerable<Flight>> GetPageAsync(int page, int size, CancellationToken ct)
    {
        return await _context.Flights
            .AsNoTracking()
            .AsSplitQuery()
            .Include(f => f.Seats)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.Passengers)
            .OrderBy(f => f.DepartureTime)
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

    public async Task<bool> AddAsync(Flight entityToAdd, CancellationToken ct = default)
    {
        try
        {
            await _context.Flights.AddAsync(entityToAdd, ct);
            return true; 
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Flight entityToUpdate, CancellationToken ct = default)
    {
        var res = await _context.Flights
            .Where(f => f.Id == entityToUpdate.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ff => ff.FlightNumber, entityToUpdate.FlightNumber)
                .SetProperty(ff => ff.Departure, entityToUpdate.Departure)
                .SetProperty(ff => ff.Arrival, entityToUpdate.Arrival)
                .SetProperty(ff => ff.DepartureTime, entityToUpdate.DepartureTime)
                .SetProperty(ff => ff.ArrivalTime, entityToUpdate.ArrivalTime)
                .SetProperty(ff => ff.AirplaneType, entityToUpdate.AirplaneType), cancellationToken: ct);

        if (res <= 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id, cancellationToken: ct);
        
        if(flight is null) return false;

        _context.Flights.Remove(flight);
        return true;
    }

    public async Task<bool> DeleteAllAsync(CancellationToken ct = default)
    {
        var res = await _context.Flights.ExecuteDeleteAsync(cancellationToken: ct);
        return res >= 0;
    }
}

