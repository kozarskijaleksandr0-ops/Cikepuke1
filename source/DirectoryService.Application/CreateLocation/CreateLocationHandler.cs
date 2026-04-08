using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared;
using Infrastructure.PostgreSQL;

namespace DirectoryService.Application.CreateLocation; 

public sealed class CreateLocationHandler
{
    private readonly ILocationRepository _repository;

    public CreateLocationHandler(ILocationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateLocationCommand command, CancellationToken ct = default)
    {
         DateTime Date = DateTime.UtcNow;
        // 1. Валидация входных данных
        if (string.IsNullOrWhiteSpace(command.Name))
        {

            throw new ArgumentException("Название не может быть пустым");
        }


        if (string.IsNullOrWhiteSpace(command.Address))
        {

            throw new ArgumentException("Адрес не может быть пустым");
        }


        if (string.IsNullOrWhiteSpace(command.TimeZone))
        {

            throw new ArgumentException("Часовой пояс не может быть пустым");
        }

        // 2. Проверка на уникальность названия

        bool exists = await _repository.Exists(command.Name, ct);
        if (exists)
        {

            throw new InvalidOperationException($"Локация с названием '{command.Name}' уже существует");
        }

        // 3. Создание объекта Location

        LocationId locationId = LocationId.Create(Guid.NewGuid());
        LocationName name = LocationName.Create(command.Name);
        LocationAddress address = LocationAddress.Create(command.Address);
        IanaTimeZone timeZone = IanaTimeZone.Create(command.TimeZone);
        EntityLifeTime lifeTime = EntityLifeTime.Create(Date, Date);

        Location location = new Location(locationId, address, name, timeZone, lifeTime);

        // 4. Сохранение в базе данных
        await _repository.Add(location, ct);

        return locationId.Value;
    }
}

