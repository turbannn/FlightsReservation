using Microsoft.EntityFrameworkCore;
using FlightsReservation.DAL.EF;
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

    public async Task<IEnumerable<Flight>> GetPageAsync(int page, int size)
    {
        return await _context.Flights
            .AsNoTracking()
            .Include(f => f.Seats)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.Passengers)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Flights.CountAsync();
    }

    public async Task<Flight?> GetByIdAsync(int id)
    {
        var flight = await _context.Flights
            .AsNoTracking()
            .Include(f => f.Seats)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.Passengers)
            .FirstOrDefaultAsync(p => p.Id == id);

        return flight;
    }

    public async Task AddAsync(Flight entityToAdd)
    {
        await _context.Flights.AddAsync(entityToAdd);
    }

    public Task UpdateAsync(Flight entityToUpdate)
    {
        _context.Flights.Update(entityToUpdate);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id);
        
        if(flight is null) return;

        _context.Flights.Remove(flight);
    }
}

