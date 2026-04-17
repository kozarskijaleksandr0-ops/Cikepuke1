using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.LocationsContext
{
	public class Location
	{
		public LocationId Id { get; }
		public LocationName Name { get; set; }
		public LocationAddress Address { get; set;}
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

		public void ChangeName(LocationName newName)
		{
			if (!LifeTime.IsActive)
			{
				throw new InvalidOperationException("Сущность удалена");
			}

			Name = newName;
			LifeTime = LifeTime.Update();
		}

		public void  ChangeAddress(LocationAddress newAddress)
		{
			if (!LifeTime.IsActive)
			{
				throw new InvalidOperationException("Сущность удалена");
			}
			
			Address = newAddress;
			LifeTime = LifeTime.Update();
	}
	    public void Delete()
    {
        if (!LifeTime.IsActive)
            {
                throw new InvalidOperationException("Локация уже удалена");
            }

            LifeTime = EntityLifeTime.Create(LifeTime.CreatedAt, DateTime.UtcNow);
	
 }
 public void Update(LocationName? newName, LocationAddress? newAddress)
{
    if (!LifeTime.IsActive)
            {
                throw new InvalidOperationException("Нельзя изменить удаленную локацию");
            }

            if (newName == null && newAddress == null)
            {
                throw new ArgumentException("Нет данных для обновления");
            }

            if (newName != null)
            {
                Name = newName;
            }

            if (newAddress != null)
            {
                Address = newAddress;
            }

            LifeTime = LifeTime.Update();
}
}}