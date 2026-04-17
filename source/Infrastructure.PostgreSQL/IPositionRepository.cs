using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.PostgreSQL;

public interface IPositionRepository
{
    Task<bool> Exists(string name, CancellationToken ct = default); 
    Task<Position?> GetById(PositionId id, CancellationToken ct = default);
    Task Add(Position position, CancellationToken ct = default);
    Task Update(Position position, CancellationToken ct = default);  
    Task Delete(PositionId id, CancellationToken ct = default);      
    Task<IEnumerable<Position>> GetAll(CancellationToken ct = default);
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
     public async Task<Position?> GetById(PositionId id, CancellationToken ct = default)
{
    return await _context.Positions.FirstOrDefaultAsync(p => p.Id == id, ct);
}

public async Task Update(Position position, CancellationToken ct = default)
{
    _context.Positions.Update(position);
    await _context.SaveChangesAsync(ct);
}

    public Task Delete(PositionId id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Position>> GetAll(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

}
