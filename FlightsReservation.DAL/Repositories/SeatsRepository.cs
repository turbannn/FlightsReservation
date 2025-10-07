using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightsReservation.DAL.Repositories;

public class SeatsRepository : ISeatsRepository
{
    private readonly FlightsDbContext _context;
    public SeatsRepository(FlightsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Seat?> GetByIdAsync(Guid id)
    {
        var seat = await _context.Seats
            .AsNoTracking()
            .Include(s => s.Passenger)
            .FirstOrDefaultAsync(p => p.Id == id);

        return seat;
    }

    //Non separate transaction
    public async Task AddAsync(Seat entityToAdd)
    {
        await _context.Seats.AddAsync(entityToAdd);
    }

    //Non separate transaction
    public Task UpdateAsync(Seat entityToUpdate)
    {
        _context.Seats.Update(entityToUpdate);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Seats.Where(s => s.Id == id).ExecuteDeleteAsync();
    }

    public async Task MarkSeatAsAvailable(Guid seatId)
    {
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId);
        if (seat != null)
        {
            seat.IsAvailable = true;
        }
    }

    public async Task MarkSeatAsOccupied(Guid seatId)
    {
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId);
        if (seat != null)
        {
            seat.IsAvailable = false;
        }
    }
}