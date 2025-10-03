using FlightsReservation.DAL.EF;
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

    public async Task<Passenger?> GetByIdAsync(int id)
    {
        var p = await _context.Passengers
            .AsNoTracking()
            .Include(p => p.Seat)
            .Include(p => p.Reservation)
            .FirstOrDefaultAsync(p => p.Id == id);

        return p;
    }

    public async Task AddAsync(Passenger entityToAdd)
    {
        await _context.Passengers.AddAsync(entityToAdd);
    }

    public Task UpdateAsync(Passenger entityToUpdate)
    {
        _context.Passengers.Update(entityToUpdate);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var p = await _context.Passengers.FirstOrDefaultAsync(p => p.Id == id);
        if(p is not null)
            _context.Passengers.Remove(p);
    }
}

