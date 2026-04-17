using DirectoryService.Domain.LocationsContext.ValueObjects;
using Infrastructure.PostgreSQL;

namespace DirectoryService.Application.CreateLocation;

public sealed class UpdateLocationHandler
{
    private readonly ILocationRepository _repository;

    public UpdateLocationHandler(ILocationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(UpdateLocationCommand command, CancellationToken ct = default)
    {
        // 1. Получаем сущность по ID
        LocationId locationId = LocationId.Create(command.Id);
        var location = await _repository.GetById(locationId, ct);
        
        if (location == null)
        {

            throw new InvalidOperationException($"Локация с ID {command.Id} не найдена");
        }


        if (!location.LifeTime.IsActive)
        {

            throw new InvalidOperationException($"Локация с ID {command.Id} не найдена");
        }

        // 2. Проверяем, что есть данные для обновления

        if (string.IsNullOrWhiteSpace(command.NewName) && string.IsNullOrWhiteSpace(command.NewAddress))
        {

            throw new ArgumentException("Нет данных для обновления");
        }

        // 3. Проверяем уникальность нового имени (если имя меняется)

        if (!string.IsNullOrWhiteSpace(command.NewName))
        {
            bool exists = await _repository.Exists(command.NewName, ct);
            if (exists)
            {

                throw new InvalidOperationException($"Локация с названием '{command.NewName}' уже существует");
            }

        }

        // 4. Создаем Value Objects (если есть данные)
        LocationName? newName = null;
        LocationAddress? newAddress = null;
        
        if (!string.IsNullOrWhiteSpace(command.NewName))
        {
            newName = LocationName.Create(command.NewName);
        }


        if (!string.IsNullOrWhiteSpace(command.NewAddress))
        {
            newAddress = LocationAddress.Create(command.NewAddress);
        }

        // 5. Обновляем локацию

        location.Update(newName, newAddress);

        // 6. Сохраняем изменения
        await _repository.Update(location, ct);

        return location.Id.Value;
    }
}
