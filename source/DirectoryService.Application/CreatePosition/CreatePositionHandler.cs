using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;
using Infrastructure.PostgreSQL;

namespace DirectoryService.Application.CreatePosition;

public sealed class CreatePositionHandler
{
     private readonly IPositionRepository _repository;

    public CreatePositionHandler(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreatePositionCommand command, CancellationToken ct = default)
    {
        DateTime Date = DateTime.UtcNow;
        // 1. Валидация входных данных
        if (string.IsNullOrWhiteSpace(command.Name))
        {

            throw new ArgumentException("Название не может быть пустым");
        }


        if (string.IsNullOrWhiteSpace(command.Description))
        {

            throw new ArgumentException("Описание не может быть пустым");
        }

        // 2. Проверка на уникальность названия

        bool exists = await _repository.Exists(command.Name, ct);
        if (exists)
        {

            throw new InvalidOperationException($"Должность с названием '{command.Name}' уже существует");
        }

        // 3. Создание объекта Position

        PositionId positionId = PositionId.Create(Guid.NewGuid());
        PositionName name = PositionName.Create(command.Name);
        PositionDescription description = PositionDescription.Create(command.Description);
        EntityLifeTime lifeTime = EntityLifeTime.Create(Date,Date);

        Position position = new Position(positionId, name, description, lifeTime);

        // 4. Сохранение в базе данных
        await _repository.Add(position, ct);

        return positionId.Value;
    }
}

