using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.PostgreSQL;

public sealed class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;

    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Exists(string name, CancellationToken ct = default)
    {
        LocationName locationName = LocationName.Create(name);
        return await _context.Locations.AnyAsync(l => l.Name == locationName, ct);
    }

    public async Task<Location?> GetById(LocationId id, CancellationToken ct = default)
    {
        return await _context.Locations.FirstOrDefaultAsync(l => l.Id == id, ct);
    }

    public async Task Add(Location location, CancellationToken ct = default)
    {
        await _context.Locations.AddAsync(location, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Update(Location location, CancellationToken ct = default)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Delete(LocationId id, CancellationToken ct = default)
    {
        var location = await GetById(id, ct);
        if (location != null)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<IEnumerable<Location>> GetAll(CancellationToken ct = default)
    {
        return await _context.Locations.ToListAsync(ct);
    }
}