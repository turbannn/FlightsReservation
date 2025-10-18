using FlightsReservation.DAL.Data;
using FlightsReservation.DAL.Entities.Model;
using FlightsReservation.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightsReservation.DAL.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly FlightsDbContext _context;
    public UsersRepository(FlightsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Reservation)
            .ThenInclude(r => r.Flight) //possible null
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);

        return user;
    }

    public async Task<bool> AddAsync(User entityToAdd, CancellationToken ct)
    {
        try
        {
            await _context.Users.AddAsync(entityToAdd, ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Update(User entityToUpdate)
    {
        _context.Users.Update(entityToUpdate);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var res = await _context.Users.Where(s => s.Id == id).ExecuteDeleteAsync(cancellationToken: ct);

        if (res <= 0)
            return false;

        return true;
    }
}