using Domain.LocationsContext.ValueObjects;

namespace Domain.Location
{
    public class Location
    {
        public LocationId Id { get; }
        public LocationName Name { get; }
        public LocationAddress Address { get; }
        public IanaTimeZone TimeZone { get; }
        public EntityLifeTime LifeTime { get; }

        public Location(
            LocationId id,
            LocationAddress address,
            LocationName name,
            IanaTimeZone timeZone,
            EntityLifeTime lifeTime)
        {
            Id = id;
            Address = address;
            Name = name;
            TimeZone = timeZone;
            LifeTime = lifeTime;
        }
    }
}

