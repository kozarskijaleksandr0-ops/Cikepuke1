using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.LocationsContext
{
	public class Location
	{
		public LocationId Id { get; }
		public LocationName Name { get; }
		public LocationAddress Address { get; }
		public IanaTimeZone TimeZone { get; set; }
		public EntityLifeTime LifeTime { get; set; }

		public Location(
			LocationId id,
			LocationAddress address,
			LocationName name,
			IanaTimeZone timeZone,
			EntityLifeTime lifeTime
		)
		{
			Id = id;
			Address = address;
			Name = name;
			TimeZone = timeZone;
			LifeTime = lifeTime;
		}

		public void ChangeIanaTimeZone(IanaTimeZone newname)
		{
			if (!LifeTime.IsActive)
			{
				throw new InvalidOperationException("Сущность удалена");
			}

			TimeZone = newname;
			LifeTime = LifeTime.Update();
		}
	}
}
