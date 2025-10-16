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
    public async Task<bool> AddAsync(Seat entityToAdd, CancellationToken ct = default)
    {
        try
        {
            await _context.Seats.AddAsync(entityToAdd, ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    //Non separate transaction
    public async Task<bool> UpdateAsync(Seat entityToUpdate, CancellationToken ct = default)
    {
        var res = await _context.Seats
            .Where(s => s.Id == entityToUpdate.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ss => ss.SeatNumber, entityToUpdate.SeatNumber)
                .SetProperty(ss => ss.IsAvailable, entityToUpdate.IsAvailable)
                .SetProperty(ss => ss.FlightId, entityToUpdate.FlightId), cancellationToken: ct);

        if (res <= 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var res = await _context.Seats.Where(s => s.Id == id).ExecuteDeleteAsync(cancellationToken: ct);

        if (res <= 0) 
            return false;

        return true;
    }

    public async Task MarkSeatAsAvailable(Guid seatId, CancellationToken ct = default)
    {
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId, cancellationToken: ct);

        if (seat is null)
        {
            throw new InvalidOperationException("Seat not found");
        }

        if (seat.IsAvailable)
        {
            throw new ArgumentException("Seat is already available");
        }

        seat.IsAvailable = true;
    }

    public async Task MarkSeatAsOccupied(Guid seatId, CancellationToken ct = default)
    {
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId, cancellationToken: ct);

        if (seat is null)
        {
            throw new InvalidOperationException("Seat not found");
        }

        if (!seat.IsAvailable)
        {
            throw new ArgumentException("Seat is already occupied");
        }

        seat.IsAvailable = false;
    }

    public async Task<bool> SetLockAsync(Guid id, CancellationToken ct = default)
    {
        var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == id, cancellationToken: ct);

        if (seat is null)
        {
            throw new InvalidOperationException("Seat not found");
        }

        if (DateTime.UtcNow < seat.Lock)
        {
            throw new ArgumentException("Seat is already locked");
        }

        seat.Lock = DateTime.UtcNow.AddMinutes(10);

        return true;
    }
}