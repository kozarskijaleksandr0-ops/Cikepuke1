
using DirectoryService.Domain.DepartmentsContext.ValueObjects;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;

namespace DirectoryService.Domain.DepartmentsContext
{
	public class DepartmentPosition
	{
		public DepartmentId DepartmentId { get; }
		public PositionId PositionId { get; }
		public Position Position { get; }
		public Department Department { get; }

		public DepartmentPosition(
		DepartmentId id,
		PositionId positionId,
		Position position,
		Department department
		)
		{
			DepartmentId = id;
			PositionId = positionId;
			Position = position;
			Department = department;
		}
	}
}
