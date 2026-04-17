using DirectoryService.Domain.Positions.ValueObjects;
using Infrastructure.PostgreSQL;

namespace DirectoryService.Application.CreatePosition;

public class RenamePositionHandler
{
     private readonly IPositionRepository _repository;
    
        public RenamePositionHandler(IPositionRepository repository)
        {
            _repository = repository;
        }
    
        public async Task<Guid> Handle(RenamePositionCommand command, CancellationToken ct = default)
        {
            // 1. Получаем сущность по ID
            PositionId positionId = PositionId.Create(command.Id);
            var position = await _repository.GetById(positionId, ct);
            
            if (position == null)
        {
            throw new InvalidOperationException($"Должность с ID {command.Id} не найдена");
        }

        if (!position.LifeTime.IsActive)
        {
            throw new InvalidOperationException($"Должность с ID {command.Id} не найдена");
        }

        // 2. Проверяем уникальность нового имени
        bool exists = await _repository.Exists(command.NewName, ct);
            if (exists)
        {
            throw new InvalidOperationException($"Должность с названием '{command.NewName}' уже существует");
        }

        // 3. Обновляем имя
        PositionName newName = PositionName.Create(command.NewName);
            position.ChangePositionName(PositionName.Create("Senior Dev"));
    
            // 4. Сохраняем изменения
            await _repository.Update(position, ct);
    
            return position.Id.Value;
        }
}
