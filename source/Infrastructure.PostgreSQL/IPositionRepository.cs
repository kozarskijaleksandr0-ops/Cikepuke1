using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;

namespace Infrastructure.PostgreSQL;

public interface IPositionRepository
{
    Task<bool> Exists(string name, CancellationToken ct = default);
    Task Add(Position position, CancellationToken ct = default);
}
public class IIPositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _context;

    public IIPositionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Exists(string name, CancellationToken ct = default)
    {
         return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
    .AnyAsync(_context.Positions, l => l.Name.Value == name, ct);

    }

    public async Task Add(Position position, CancellationToken ct = default)
    {
        await _context.Positions.AddAsync(position, ct);
        await _context.SaveChangesAsync(ct);
    }
}
