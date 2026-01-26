using Domain.LocationsContext.ValueObjects;
using Domain.Position.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static Domain.LocationsContext.ValueObjects.LocationAdderss;

namespace Domain.LocationsContext;
public class Location
{
    public Location (Locationid id, LocationName name, LocationAddress address, EntityLifeTime lifeTime, IanaTimeZone timeZone)
    {
        Id = id;
        Name = name;
        Address = address;
        LifeTime = lifeTime;
        TimeZone = timeZone;
    }
    public Locationid Id { get; }
    public LocationName Name { get; }
    public LocationAddress Address { get; }
    public EntityLifeTime LifeTime { get; }
    public IanaTimeZone TimeZone { get; }
}
