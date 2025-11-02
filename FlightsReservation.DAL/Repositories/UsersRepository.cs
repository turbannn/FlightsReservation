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

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Users.AsNoTracking().ToListAsync(ct);
    }

    public async Task<User?> GetByLoginAndPasswordAsync(string login, string password, CancellationToken ct)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Username == login && p.Password == password, cancellationToken: ct);

        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Reservations)
            .ThenInclude(r => r.Flight)
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
    public async Task<bool> UpdateMoneyAsync(Guid id, int amount, CancellationToken ct)
    {
        var res = await _context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u.SetProperty(user => user.Money, amount), cancellationToken: ct);

        if (res <= 0)
            return false;

        return true;
    }

    public async Task<bool> AddMoneyAsync(Guid id, int amount, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);

        if (user is null)
            return false;

        user.Money += amount;

        return true;
    }
    public async Task<bool> SubtractMoneyAsync(Guid id, double amount, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: ct);

        if (user is null || user.Money < amount)
            return false;

        user.Money -= (int)amount;

        return true;
    }
}