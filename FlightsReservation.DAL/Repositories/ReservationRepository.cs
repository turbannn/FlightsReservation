
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
    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        var r = await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Flight)
            .Include(r => r.Passengers)
            .ThenInclude(p => p.Seat)
            .FirstOrDefaultAsync(r => r.Id == id);

        return r;
    }

    public async Task AddAsync(Reservation entityToAdd)
    {
        await _context.Reservations.AddAsync(entityToAdd);
    }

    public Task UpdateAsync(Reservation entityToUpdate)
    {
        _context.Reservations.Update(entityToUpdate);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Reservations.Where(r => r.Id == id).ExecuteDeleteAsync();
    }
}