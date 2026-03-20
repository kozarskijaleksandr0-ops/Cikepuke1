using DirectoryService.Domain.DepartmentsContext.ValueObjects;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;

namespace DirectoryService.Domain.DepartmentsContext;

public class DepartmentLocation
{
    public DepartmentId DepartmentId { get; }
		public LocationId LocationId { get; }
		public Location Location { get; }
		public Department Department { get; }

		public DepartmentLocation(
		DepartmentId id,
		LocationId locationId,
		Location location,
		Department department
		)
		{
			DepartmentId = id;
			LocationId = locationId;
			Location = location;
			Department = department;
		}
}


