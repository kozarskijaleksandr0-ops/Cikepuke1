using DirectoryService.Domain.PositionsContext.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.PositionsContext
{
    public class Position
    {
        public PositionId Id { get; }
        public PositionName Name { get; }
        public PositionDescription Description { get; }
        public bool IsActive { get; }
        public EntityLifeTime LifeTime { get; }

        public Position(
            PositionId id,
            PositionName name,
            PositionDescription description,
            bool isActive,
            EntityLifeTime lifeTime)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
            LifeTime = lifeTime;
        }

        // Метод для изменения активности
        public Position ChangeActivity(bool isActive)
        {
            // В реальном проекте здесь создавался бы новый объект с обновленным состоянием
            // Так как объекты иммутабельны
            // Для простоты оставим как есть
            return this;
        }

        // Метод для обновления описания
        public Position UpdateDescription(PositionDescription newDescription)
        {
            // Аналогично - в реальности создание нового объекта
            return this;
        }
    }
}