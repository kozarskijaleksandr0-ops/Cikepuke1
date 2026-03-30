using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared;


namespace WebApi2;

public static class LocationStorage
{
    private static readonly Dictionary<LocationId, Location> _locations = [];

    public static void Add(Location location)
    {
        ArgumentNullException.ThrowIfNull(location);

        if (_locations.ContainsKey(location.Id))
        {
            throw new InvalidOperationException($"Локация с Id {location.Id.Value} уже существует");
        }

        if (_locations.Any(x => x.Value.Name.Value == location.Name.Value))
        {
            throw new InvalidOperationException($"Локация с названием {location.Name.Value} уже существует");
        }

        _locations.Add(location.Id, location);
    }

    public static Location? GetById(LocationId id)
    {
        _locations.TryGetValue(id, out Location? location);
        return location;
    }

    public static IEnumerable<Location> GetAll()
    {
        return _locations.Values;
    }

    public static void Remove(Location location)
    {
        ArgumentNullException.ThrowIfNull(location);
        _locations.Remove(location.Id);
    }
    public record UpdateLocationRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? LifeTime { get; set; }
    }
     public static void Update(Location location)
    {
        ArgumentNullException.ThrowIfNull(location);

        if (!_locations.ContainsKey(location.Id))
        {
            throw new InvalidOperationException($"Локация с Id {location.Id.Value} не найдена");
        
        }   
        // Проверка на уникальность имени (если имя изменилось)
        if (_locations.Any(x => x.Value.Name.Value == location.Name.Value && x.Key != location.Id))
        {
            throw new InvalidOperationException($"Локация с названием {location.Name.Value} уже существует");         
        }    
    }
     public static void HardRemove(LocationId id)
    {
        if (!_locations.ContainsKey(id))
        {
            throw new InvalidOperationException($"Локация с Id {id.Value} не найдена");
        }   
        _locations.Remove(id);
    }
    
    



    public static void InitializeStorage()
    {
        DateTime Date = DateTime.UtcNow;
        Location[] locations = new[]
        {
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Москва, ул. Тверская, 1"),
                LocationName.Create("Главный офис Москва"),
                IanaTimeZone.Create("Europe/Moscow"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Санкт-Петербург, Невский пр., 10"),
                LocationName.Create("Северо-Западный филиал"),
                IanaTimeZone.Create("Europe/Moscow"),
                EntityLifeTime.Create(Date,Date)
            ),

            new Location(
                LocationId.Create(),
                LocationAddress.Create("Казань, ул. Баумана, 15"),
                LocationName.Create("Казанский офис"),
                IanaTimeZone.Create("Europe/Moscow"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Екатеринбург, ул. Ленина, 25"),
                LocationName.Create("Уральский филиал"),
                IanaTimeZone.Create("Asia/Yekaterinburg"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Владивосток, ул. Светланская, 30"),
                LocationName.Create("Дальневосточный филиал"),
                IanaTimeZone.Create("Asia/Vladivostok"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Новосибирск, Красный пр., 50"),
                LocationName.Create("Сибирский филиал"),
                IanaTimeZone.Create("Asia/Novosibirsk"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Нижний Новгород, ул. Большая Покровская, 20"),
                LocationName.Create("Приволжский филиал"),
                IanaTimeZone.Create("Europe/Moscow"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Ростов-на-Дону, ул. Большая Садовая, 40"),
                LocationName.Create("Южный филиал"),
                IanaTimeZone.Create("Europe/Moscow"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Самара, ул. Молодогвардейская, 100"),
                LocationName.Create("Самарский офис"),
                IanaTimeZone.Create("Europe/Samara"),
                EntityLifeTime.Create(Date,Date)
            ),
            new Location(
                LocationId.Create(),
                LocationAddress.Create("Челябинск, ул. Кирова, 80"),
                LocationName.Create("Челябинский филиал"),
                IanaTimeZone.Create("Asia/Yekaterinburg"),
                EntityLifeTime.Create(Date,Date)
            )
        };

        foreach (Location location in locations)
        {
            _locations.Add(location.Id, location);
        }
    }
}

