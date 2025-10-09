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

    public async Task<Seat?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var seat = await _context.Seats
            .AsNoTracking()
            .Include(s => s.Passenger)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);

        return seat;
    }

    //Non separate transaction
    public async Task AddAsync(Seat entityToAdd, CancellationToken ct = default)
    {
        await _context.Seats.AddAsync(entityToAdd, ct);
    }

    //Non separate transaction
    public async Task UpdateAsync(Seat entityToUpdate, CancellationToken ct = default)
    {
        await _context.Seats
            .Where(s => s.Id == entityToUpdate.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ss => ss.SeatNumber, entityToUpdate.SeatNumber)
                .SetProperty(ss => ss.IsAvailable, entityToUpdate.IsAvailable)
                .SetProperty(ss => ss.FlightId, entityToUpdate.FlightId), cancellationToken: ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _context.Seats.Where(s => s.Id == id).ExecuteDeleteAsync(cancellationToken: ct);
    }

    public async Task MarkSeatAsAvailable(Guid seatId, CancellationToken ct = default)
    {
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId, cancellationToken: ct);
        if (seat != null)
        {
            seat.IsAvailable = true;
        }
    }

    public async Task MarkSeatAsOccupied(Guid seatId, CancellationToken ct = default)
    {
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId, cancellationToken: ct);
        if (seat != null)
        {
            seat.IsAvailable = false;
        }
    }
}