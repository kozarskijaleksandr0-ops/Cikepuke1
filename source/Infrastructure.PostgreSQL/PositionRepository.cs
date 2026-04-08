using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.PostgreSQL;

public sealed class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _context;

    public PositionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Exists(string name, CancellationToken ct = default)
    {
        PositionName positionName = PositionName.Create(name);
        return await _context.Positions.AnyAsync(p => p.Name == positionName, ct);
    }

    public async Task<Position?> GetById(PositionId id, CancellationToken ct = default)
    {
        return await _context.Positions.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task Add(Position position, CancellationToken ct = default)
    {
        await _context.Positions.AddAsync(position, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Update(Position position, CancellationToken ct = default)
    {
        _context.Positions.Update(position);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Delete(PositionId id, CancellationToken ct = default)
    {
        var position = await GetById(id, ct);
        if (position != null)
        {
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<IEnumerable<Position>> GetAll(CancellationToken ct = default)
    {
        return await _context.Positions.ToListAsync(ct);
    }

}