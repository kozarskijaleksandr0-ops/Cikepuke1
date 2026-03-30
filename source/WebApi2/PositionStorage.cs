using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;

namespace WebApi2;

public static class PositionStorage
{
    private static readonly Dictionary<PositionId, Position> _positions = [];

    public static void Add(Position position)
    {
        ArgumentNullException.ThrowIfNull(position);

        if (_positions.ContainsKey(position.Id))
        {
            throw new InvalidOperationException($"Должность с Id {position.Id.Value} уже существует");
        }

        if (_positions.Any(x => x.Value.Name.Value == position.Name.Value))
        {
            throw new InvalidOperationException($"Должность с названием {position.Name.Value} уже существует");
        }

        _positions.Add(position.Id, position);
    }

    public static Position? GetById(PositionId id)
    {
        _positions.TryGetValue(id, out Position? position);
        return position;
    }

    public static IEnumerable<Position> GetAll()
    {
        return _positions.Values.Where(p => p.LifeTime.IsActive == false);
    }

    public static void Update(Position position)
    {
        ArgumentNullException.ThrowIfNull(position);

        if (!_positions.ContainsKey(position.Id))
        {
            throw new InvalidOperationException($"Должность с Id {position.Id.Value} не найдена");
        }

        if (_positions.Any(x => x.Value.Name.Value == position.Name.Value && x.Key != position.Id))
        {
            throw new InvalidOperationException($"Должность с названием {position.Name.Value} уже существует");
        }

        _positions[position.Id] = position;
    }

    public static void HardRemove(PositionId id)
    {
        if (!_positions.ContainsKey(id))
        {
            throw new InvalidOperationException($"Должность с Id {id.Value} не найдена");
        }

        _positions.Remove(id);
    }

    public static void InitializeStorage()
    {
        DateTime Date = DateTime.UtcNow;
        Position[] positions = new[]
        {
            new Position(
                PositionId.Create(Guid.NewGuid()),
                PositionName.Create("Генеральный директор"),
                PositionDescription.Create("Руководство компанией, определение стратегии развития"),
                EntityLifeTime.Create(Date, Date)
            ),
            new Position(
                PositionId.Create(Guid.NewGuid()),
                PositionName.Create("Финансовый директор"),
                PositionDescription.Create("Управление финансами компании, бюджетирование"),
                EntityLifeTime.Create(Date, Date)
            ),
            new Position(
                PositionId.Create(Guid.NewGuid()),
                PositionName.Create("Технический директор"),
                PositionDescription.Create("Управление разработкой, техническая стратегия"),
                EntityLifeTime.Create(Date, Date)
            ),
            new Position(
                PositionId.Create(Guid.NewGuid()),
                PositionName.Create("Руководитель отдела продаж"),
                PositionDescription.Create("Управление отделом продаж, развитие клиентской базы"),
                EntityLifeTime.Create(Date, Date)
            ),
            new Position(
                PositionId.Create(Guid.NewGuid()),
                PositionName.Create("Ведущий разработчик"),
                PositionDescription.Create("Разработка архитектуры, код-ревью, наставничество"),
                EntityLifeTime.Create(Date, Date)
            ),
            new Position(
                PositionId.Create(Guid.NewGuid()),
                PositionName.Create("Разработчик"),
                PositionDescription.Create("Разработка программного обеспечения"),
                EntityLifeTime.Create(Date, Date)
            )
        };

        foreach (Position position in positions)
        {
            _positions.Add(position.Id, position);
        }
    }
}