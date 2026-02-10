using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Positions
{
	public class Position
	{
		public PositionId Id { get; }
		public PositionName Name { get; set; }
		public PositionDescription Description { get; }
		public bool IsActive { get; }
		public EntityLifeTime LifeTime { get; set; }

		public Position(
			PositionId id,
			PositionName name,
			PositionDescription description,
			bool isActive,
			EntityLifeTime lifeTime
		)
		{
			Id = id;
			Name = name;
			Description = description;
			IsActive = isActive;
			LifeTime = lifeTime;
		}

		public void ChangePositionName(PositionName newname)
		{
			if (!LifeTime.IsActive)
			{
				throw new InvalidOperationException("Сущность удалена");
			}
			Name = newname;
			LifeTime = LifeTime.Update();
		}
	}
}
