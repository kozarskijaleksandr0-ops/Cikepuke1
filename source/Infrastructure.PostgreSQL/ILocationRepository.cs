using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.PostgreSQL;

public interface ILocationRepository
{
    Task<bool> Exists(string name, CancellationToken ct = default);
    Task<Location?> GetById(LocationId id, CancellationToken ct = default);
    Task Add(Location location, CancellationToken ct = default);
    Task Update(Location location, CancellationToken ct = default);
    Task Delete(LocationId id, CancellationToken ct = default);
    Task<IEnumerable<Location>> GetAll(CancellationToken ct = default);

}    