using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Positions
{
	public class Position
	{
		public PositionId Id { get; }
		public PositionName Name { get; set; }
		public PositionDescription Description { get; set; }
		public bool IsActive { get; }
		public EntityLifeTime LifeTime { get; set; }

		public Position(
			PositionId id,
			PositionName name,
			PositionDescription description,
			EntityLifeTime lifeTime
		)
		{
			Id = id;
			Name = name;
			Description = description;			
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
		  public void ChangeDescription(PositionDescription newDescription)
    {
        if (!LifeTime.IsActive)
    {
        throw new InvalidOperationException("Нельзя изменить удаленную должность");
    }

    Description = newDescription;
    }

    public void Delete()
    {
        if (!LifeTime.IsActive)
            {
                throw new InvalidOperationException("Должность уже удалена");
            }

            LifeTime = EntityLifeTime.Create(LifeTime.CreatedAt, DateTime.UtcNow);
    }
	}
	
}
